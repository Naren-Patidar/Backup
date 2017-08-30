using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using CCODundeeApplication.CustomerService;
using CCODundeeApplication.ClubcardService;
using CCODundeeApplication.PreferenceServices;
using CCODundeeApplication.JoinLoyaltyService;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;

namespace CCODundeeApplication
{
    /// <summary>
    /// Your preferences
    /// Purpose: Customer can view and update his/her preferences here.
    /// <para>Author: Sadanand</para>
    /// <para>Date Created : 26/03/2010</para>
    /// </summary>
    public partial class Preferences : System.Web.UI.Page
    {
        #region <Local variables>
        //Declare local varibales.
        string resultXml = string.Empty;
        string errorXml = string.Empty;
        string objName = string.Empty;
        string methodName = string.Empty;
        long customerID = 0;
        DataSet dsCustomerPreference = null;
        DataSet dsHHCustomers = null;
        XmlDocument resulDoc = null;
        CustomerServiceClient serviceClient = null;
        string conditionXML = string.Empty;
        DataSet dsConfigDetails = new DataSet();
        CustomerServiceClient customerObj = null;
        ClubcardServiceClient clubcardObj = null;
        JoinLoyaltyServiceClient joinServiceClient = null;
        string culture = ConfigurationManager.AppSettings["Culture"];
        string customerTitle = string.Empty;
        string customerFirstName = string.Empty;
        string customerLastName = string.Empty;
        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        DateTime strSaveTreeConfigDate = Convert.ToDateTime(ConfigurationManager.AppSettings["SaveTreePrefStartDate"].ToString());
        bool isGridFormat = Convert.ToBoolean(ConfigurationManager.AppSettings["IsGridFormat"]);
        bool isOptInBehavior = Convert.ToBoolean(ConfigurationManager.AppSettings["IsOptInBehavior"]);
        bool isAviosMembershipEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableAviosMembership"]);
        int[] aviosLength = Array.ConvertAll<string, int>(ConfigurationManager.AppSettings["LengthforAvios"].Split(','), Convert.ToInt32);
        int[] baAviosLength = Array.ConvertAll<string, int>(ConfigurationManager.AppSettings["LengthforBAAvios"].Split(','), Convert.ToInt32);
        int[] virginALength = Array.ConvertAll<string, int>(ConfigurationManager.AppSettings["LengthforVirginA"].Split(','), Convert.ToInt32);
        bool flgShowSaveTrees;
        PreferenceServiceClient preferenceserviceClient = null;
        string sdefaultpreferenceType = string.Empty;
        protected string errMessageClubcardStatement = string.Empty;
        protected string errMessageAviosMembership = string.Empty;
        protected string errMessageClubcardStatementVirgin = string.Empty;
        protected string errMessageClubcardStatementPre = string.Empty;
        protected string errMessage = string.Empty;
        protected string errAssoMessage = string.Empty;
        protected string errMessageBnTemail = "Please enter a valid email address";
        protected string errMessageBnTMobile = "Please enter a valid mobile phone number";
        protected string errMessageBTemail = string.Empty;
        protected string errMessageBTMobile = string.Empty;

        int iChildNumberIn = 0;
        int iChildNumberOut = 0;

        int iChildAssoNumberIn = 0;
        int iChildAssoNumberOut = 0;

        ClubcardServiceClient clubcardserviceClient = null;
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Preferences.Page_Load()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Preferences.Page_Load()");
                #endregion

                txtAviosMembership.MaxLength = Convert.ToInt16(aviosLength[1].ToString());
                txtBAvios.MaxLength = Convert.ToInt16(baAviosLength[1].ToString());
                txtVirgnMembershipID.MaxLength = Convert.ToInt16(virginALength[1].ToString());

                int rowNumber = 99;
                //******* Release 1.6 changes start (Modified in release1.8 to fix save tree visible to CSC) *********//
                if (DateTime.Now.Date >= strSaveTreeConfigDate)
                {
                    flgShowSaveTrees = true;
                }
                //******* Release 1.6 changes end *********//

                if (!IsPostBack)
                {
                    #region RoleCapabilityImplementation

                    xmlCapability = new XmlDocument();
                    dsCapability = new DataSet();

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                    {
                        xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                        dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                        HtmlAnchor DataConfig = (HtmlAnchor)Master.FindControl("dataconfig");
                        Control link = (HtmlGenericControl)Master.FindControl("liDataconfig");

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
                            Label DeLinkAccount = (Label)Master.FindControl("lblDelinking");
                            HtmlAnchor customerCoupon = (HtmlAnchor)Master.FindControl("customerCoupon");
                            HtmlAnchor AccountOperationReports = (HtmlAnchor)Master.FindControl("AccReports");
                            HtmlAnchor PromotionalCodeReport = (HtmlAnchor)Master.FindControl("PromotionalCode");
                            HtmlAnchor PointsEarnedReports = (HtmlAnchor)Master.FindControl("PointsEarnedReport");
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

                            if (dsCapability.Tables[0].Columns.Contains("DeLinkingAccount") != false)
                            {
                                DeLinkAccount.Visible = true;
                            }
                            else
                            {
                                DeLinkAccount.Visible = false;
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
                            if (dsCapability.Tables[0].Columns.Contains("CreateNewCustomer") != false)
                            {
                                Join.Disabled = false;
                            }
                            else
                            {
                                Join.Disabled = true;
                                Join.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("UpdateCustomerPreferences") != false)
                            {

                            }
                            else
                            {
                                dvBody.Disabled = true;
                                btnConfirmPreferences.Attributes.Add("disabled", "true");
                            }
                        }
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID")))
                    {
                        //******* Release 1.5 changes start *********//
                        SetHouseHoldStatus();
                        //******* Release 1.5 changes end *********//

                        customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                        clubcardObj = new ClubcardServiceClient();

                        if (clubcardObj.GetHouseholdCustomers(out errorXml, out resultXml, customerID, culture))
                        {
                            resulDoc = new XmlDocument();
                            resulDoc.LoadXml(resultXml);
                            dsHHCustomers = new DataSet();
                            dsHHCustomers.ReadXml(new XmlNodeReader(resulDoc));

                            if (dsHHCustomers.Tables.Count > 0)
                            {
                                int numberOfCustomers = dsHHCustomers.Tables[0].Rows.Count;
                                hdnNoofCustomer.Value = numberOfCustomers.ToString();
                                if (numberOfCustomers > 1)
                                {
                                    divBnTAsso.Visible = true;
                                }
                                else
                                {
                                    DivBT.Attributes.Add("style", "width:100%;border-right:0px");
                                }
                                bool isAssociate = false;
                                for (int i = 0; i < dsHHCustomers.Tables[0].Rows.Count; i++)
                                {
                                    if (dsHHCustomers.Tables["HouseholdCustomers"].Rows[i]["PrimaryCustomerID"].ToString() == dsHHCustomers.Tables["HouseholdCustomers"].Rows[i]["CustomerID"].ToString())
                                    {
                                        hdnPrimaryCustID.Value = dsHHCustomers.Tables["HouseholdCustomers"].Rows[i]["CustomerID"].ToString();
                                        rowNumber = 0;
                                        if (dsHHCustomers.Tables["HouseholdCustomers"].Columns.Contains("TitleEnglish"))
                                        {
                                            customerTitle = Helper.ToTitleCase(dsHHCustomers.Tables["HouseholdCustomers"].Rows[i]["TitleEnglish"].ToString());
                                        }

                                        if (dsHHCustomers.Tables["HouseholdCustomers"].Columns.Contains("Name1"))
                                        {
                                            customerFirstName = Helper.ToTitleCase(dsHHCustomers.Tables["HouseholdCustomers"].Rows[i]["Name1"].ToString());
                                        }

                                        if (dsHHCustomers.Tables["HouseholdCustomers"].Columns.Contains("Name3"))
                                        {
                                            customerLastName = Helper.ToTitleCase(dsHHCustomers.Tables["HouseholdCustomers"].Rows[i]["Name3"].ToString());
                                        }
                                    }
                                    else if ((Convert.ToInt64(dsHHCustomers.Tables["HouseholdCustomers"].Rows[i]["CustomerID"].ToString()) == customerID) && isAssociate == false)
                                    {
                                        isAssociate = true;
                                        rowNumber = 1;
                                        hdnAssociateCustID.Value = dsHHCustomers.Tables["HouseholdCustomers"].Rows[i]["CustomerID"].ToString();
                                    }
                                    else if (((Convert.ToInt64(dsHHCustomers.Tables["HouseholdCustomers"].Rows[i]["PrimaryCustomerID"].ToString()) == customerID) && isAssociate == false))
                                    {
                                        isAssociate = true;
                                        rowNumber = 1;
                                        hdnAssociateCustID.Value = dsHHCustomers.Tables["HouseholdCustomers"].Rows[i]["CustomerID"].ToString();
                                    }
                                    if (rowNumber == 0)
                                    {
                                        LoadBnTPreferences(Convert.ToInt64(hdnPrimaryCustID.Value), 0);
                                    }
                                    else if (rowNumber == 1)
                                    {
                                        LoadBnTPreferences(Convert.ToInt64(hdnAssociateCustID.Value), 1);
                                    }
                                }
                            }
                        }

                        lblCustomerName.Text = customerTitle.Trim() + " " + customerFirstName.Trim() + " " + customerLastName.Trim();
                    }

                    //Load customer preferences
                    LoadConfigDetails();
                    LoadPreferences();
                    lblChildNo.Text = Convert.ToString(Convert.ToInt32(hdnConfigDOB.Value) / 365);
                    lbltotalChild.Text = Convert.ToString(Convert.ToInt32(hdnConfigDOB.Value) / 365);
                    lblChildNoAsso.Text = Convert.ToString(Convert.ToInt32(hdnConfigDOB.Value) / 365);
                    lblTotalChildNoAsso.Text = Convert.ToString(Convert.ToInt32(hdnConfigDOB.Value) / 365);
                    LoadCustomerDetails();
                    LoadMemmberShip();

                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Preferences.Page_Load()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Preferences.Page_Load()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Preferences.Page_Load() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Preferences.Page_Load() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Preferences.Page_Load()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
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
                Response.Redirect("~/Default.aspx", false);
        }
        #endregion
        public void LoadBabtTodlerDetails()
        {
            customerID = Convert.ToInt64(hdnPrimaryCustID.Value);
            preferenceserviceClient = new PreferenceServiceClient();
            ClubDetails objClubs = new ClubDetails();
            DataTable dtPreference = new DataTable("BTClub");
            dtPreference.Columns.Add("DateOfBirth", typeof(string), null);
            dtPreference.Columns.Add("OriginalDateOfBirth", typeof(string), null);
            dtPreference.Columns.Add("MediaID", typeof(Int16), null);
            dtPreference.Columns.Add("ClubID", typeof(Int16), null);
            dtPreference.Columns.Add("MembershipID", typeof(string), null);
            dtPreference.Columns.Add("IsDeleted", typeof(string), null);

            DataTable dtMedia = new DataTable("Media");
            dtMedia.Columns.Add("MediaID", typeof(Int16), null);
            dtMedia.Columns.Add("MediaDescription", typeof(string), null);
            objClubs = preferenceserviceClient.ViewClubDetails(customerID);
            if (objClubs != null && objClubs.ClubInformation.Count > 0)
            {
                for (int i = 0; i < objClubs.ClubInformation.Count; i++)
                {
                    if (objClubs.ClubInformation[i].ClubID == 1 && objClubs.ClubInformation[i].DateOfBirth != "" && objClubs.ClubInformation[i].IsDeleted == "N")
                    {
                        if (dtPreference.Rows.Count < 5)
                        {
                            dtPreference.Rows.Add("", objClubs.ClubInformation[i].DateOfBirth, objClubs.ClubInformation[i].Media, objClubs.ClubInformation[i].ClubID,
                                objClubs.ClubInformation[i].MembershipID, objClubs.ClubInformation[i].IsDeleted);
                        }
                    }
                    //CCMCA-4700
                    if (objClubs.ClubInformation[i].ClubID == 4 && objClubs.ClubInformation[i].IsDeleted == "N" && chkAviosStandard.Checked && isAviosMembershipEnable)
                    {
                        txtAviosMembership.Text = objClubs.ClubInformation[i].MembershipID;
                        txtBAvios.Text = "";
                        txtVirgnMembershipID.Text = "";
                    }
                    else if (objClubs.ClubInformation[i].ClubID == 4 && objClubs.ClubInformation[i].IsDeleted == "N" && chkAviosPremium.Checked && isAviosMembershipEnable)
                    {
                        txtAviosMembership.Text = objClubs.ClubInformation[i].MembershipID;
                        txtBAvios.Text = "";
                        txtVirgnMembershipID.Text = "";


                    }
                    //END
                    else if (objClubs.ClubInformation[i].ClubID == 3 && objClubs.ClubInformation[i].IsDeleted == "N" && chkBAviosStandard.Checked)
                    {
                        txtBAvios.Text = objClubs.ClubInformation[i].MembershipID;
                        txtVirgnMembershipID.Text = "";
                        txtAviosMembership.Text = "";


                    }
                    else if (objClubs.ClubInformation[i].ClubID == 3 && objClubs.ClubInformation[i].IsDeleted == "N" && chkBAviosPremium.Checked)
                    {
                        txtBAvios.Text = objClubs.ClubInformation[i].MembershipID;
                        txtVirgnMembershipID.Text = "";
                        txtAviosMembership.Text = "";


                    }
                    else if (objClubs.ClubInformation[i].ClubID == 2 && objClubs.ClubInformation[i].IsDeleted == "N" && chkVirginAtlantic.Checked)
                    {
                        txtVirgnMembershipID.Text = objClubs.ClubInformation[i].MembershipID;
                        txtBAvios.Text = "";
                        txtAviosMembership.Text = "";

                    }

                }
            }
            ViewState["BTOriginalDetails"] = dtPreference;
            ViewState["BTDetails"] = dtPreference;
            if (dtPreference.Rows.Count == 0 && chkBabyTodlerOptIn.Visible)
            {
                dtPreference.Rows.Add("", "", 0, BusinessConstants.CLUB_BT, "", "N");
                dtPreference.AcceptChanges();
            }
            grdBabyTodlerOptIn.DataSource = dtPreference.DefaultView;
            grdBabyTodlerOptIn.DataBind();
            if (grdBabyTodlerOptIn.Rows.Count >= 5)
            {
                lnkAddChild.Enabled = false;
                lnkAddChild.Attributes.Add("style", "cursor:auto");
            }
            else
            {
                lnkAddChild.Enabled = true;
            }
            if (grdBabyTodlerOptIn.Rows.Count == 0)
            {
                grdBabyTodlerOptIn.Attributes.Add("style", "display:none");
            }
            else
            {
                grdBabyTodlerOptIn.Attributes.Add("style", "display:block");
            }
            grdBabyTodlerOptOut.DataSource = dtPreference.DefaultView;
            grdBabyTodlerOptOut.DataBind();
            if (grdBabyTodlerOptOut.Rows.Count == 0)
            {
                grdBabyTodlerOptOut.Attributes.Add("style", "display:none");
            }
            else
            {
                grdBabyTodlerOptOut.Attributes.Add("style", "display:block");
            }
            if (grdBabyTodlerOptOut.Rows.Count == 5)
            {
                LinkButton2.Enabled = false;
                LinkButton2.Attributes.Add("style", "cursor:auto");
            }
            else
            {
                LinkButton2.Enabled = true;
            }
        }

        public void LoadAssoBabtTodlerDetails()
        {
            customerID = Convert.ToInt64(hdnAssociateCustID.Value);
            preferenceserviceClient = new PreferenceServiceClient();
            ClubDetails objClubs = new ClubDetails();
            DataTable dtAssoPreference = new DataTable("BTClub");
            dtAssoPreference.Columns.Add("DateOfBirth", typeof(string), null);
            dtAssoPreference.Columns.Add("OriginalDateOfBirth", typeof(string), null);
            dtAssoPreference.Columns.Add("MediaID", typeof(Int16), null);
            dtAssoPreference.Columns.Add("ClubID", typeof(Int16), null);
            dtAssoPreference.Columns.Add("MembershipID", typeof(string), null);
            dtAssoPreference.Columns.Add("IsDeleted", typeof(string), null);

            DataTable dtMedia = new DataTable("Media");
            dtMedia.Columns.Add("MediaID", typeof(Int16), null);
            dtMedia.Columns.Add("MediaDescription", typeof(string), null);
            objClubs = preferenceserviceClient.ViewClubDetails(customerID);
            if (objClubs != null && objClubs.ClubInformation.Count > 0)
            {
                for (int i = 0; i < objClubs.ClubInformation.Count; i++)
                {
                    if (objClubs.ClubInformation[i].ClubID == 1 && objClubs.ClubInformation[i].DateOfBirth != "" && objClubs.ClubInformation[i].IsDeleted == "N")
                    {
                        if (dtAssoPreference.Rows.Count < 5)
                        {
                            dtAssoPreference.Rows.Add("", objClubs.ClubInformation[i].DateOfBirth, objClubs.ClubInformation[i].Media, objClubs.ClubInformation[i].ClubID,
                                objClubs.ClubInformation[i].MembershipID, objClubs.ClubInformation[i].IsDeleted);
                        }
                    }
                    //CCMCA-4700
                    if (objClubs.ClubInformation[i].ClubID == 4 && objClubs.ClubInformation[i].IsDeleted == "N" && chkAviosStandard.Checked && isAviosMembershipEnable)
                    {
                        txtAviosMembership.Text = objClubs.ClubInformation[i].MembershipID;
                        txtBAvios.Text = "";
                        txtVirgnMembershipID.Text = "";


                    }
                    else if (objClubs.ClubInformation[i].ClubID == 4 && objClubs.ClubInformation[i].IsDeleted == "N" && chkAviosPremium.Checked && isAviosMembershipEnable)
                    {
                        txtAviosMembership.Text = objClubs.ClubInformation[i].MembershipID;
                        txtBAvios.Text = "";
                        txtVirgnMembershipID.Text = "";


                    }
                    //End
                    else if (objClubs.ClubInformation[i].ClubID == 3 && objClubs.ClubInformation[i].IsDeleted == "N" && chkBAviosStandard.Checked)
                    {
                        txtBAvios.Text = objClubs.ClubInformation[i].MembershipID;
                        txtVirgnMembershipID.Text = "";
                        txtAviosMembership.Text = "";


                    }
                    else if (objClubs.ClubInformation[i].ClubID == 3 && objClubs.ClubInformation[i].IsDeleted == "N" && chkBAviosPremium.Checked)
                    {
                        txtBAvios.Text = objClubs.ClubInformation[i].MembershipID;
                        txtVirgnMembershipID.Text = "";
                        txtAviosMembership.Text = "";


                    }
                    else if (objClubs.ClubInformation[i].ClubID == 2 && objClubs.ClubInformation[i].IsDeleted == "N" && chkVirginAtlantic.Checked)
                    {
                        txtVirgnMembershipID.Text = objClubs.ClubInformation[i].MembershipID;
                        txtBAvios.Text = "";
                        txtAviosMembership.Text = "";

                    }

                }
            }
            ViewState["BTAssoOriginalDetails"] = dtAssoPreference;
            ViewState["BTAssoDetails"] = dtAssoPreference;
            if (dtAssoPreference.Rows.Count == 0 && chkAssoBabyTodlerOptIn.Visible)
            {
                dtAssoPreference.Rows.Add("", "", 0, BusinessConstants.CLUB_BT, "", "N");
                dtAssoPreference.AcceptChanges();
            }
            grdBabyTodlerAssoOptIn.DataSource = dtAssoPreference.DefaultView;
            grdBabyTodlerAssoOptIn.DataBind();
            if (grdBabyTodlerAssoOptIn.Rows.Count >= 5)
            {
                LinkButton1.Enabled = false;
                LinkButton1.Attributes.Add("style", "cursor:auto");
            }
            else
            {
                LinkButton1.Enabled = true;
            }
            if (grdBabyTodlerAssoOptIn.Rows.Count == 0)
            {
                grdBabyTodlerAssoOptIn.Attributes.Add("style", "display:none");
            }
            else
            {
                grdBabyTodlerAssoOptIn.Attributes.Add("style", "display:block");
            }
            grdBabyTodlerAssoOptOut.DataSource = dtAssoPreference.DefaultView;
            grdBabyTodlerAssoOptOut.DataBind();
            if (grdBabyTodlerAssoOptOut.Rows.Count == 0)
            {
                grdBabyTodlerAssoOptOut.Attributes.Add("style", "display:none");
            }
            else
            {
                grdBabyTodlerAssoOptOut.Attributes.Add("style", "display:block");
            }
            if (grdBabyTodlerAssoOptOut.Rows.Count == 5)
            {
                LinkButton3.Enabled = false;
                LinkButton3.Attributes.Add("style", "cursor:auto");
            }
            else
            {
                LinkButton3.Enabled = true;
            }
        }
        /// <summary>
        /// Loads the Mailing and Statement preferences.
        /// </summary>
        /// <param name="dsCustomerPreference"></param>
        /// 

        public void LoadMemmberShip()
        {
            customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
            preferenceserviceClient = new PreferenceServiceClient();
            ClubDetails objClubs = new ClubDetails();
            DataTable dtPreference = new DataTable("BTClub");
            dtPreference.Columns.Add("DateOfBirth", typeof(string), null);
            dtPreference.Columns.Add("OriginalDateOfBirth", typeof(string), null);
            dtPreference.Columns.Add("MediaID", typeof(Int16), null);
            dtPreference.Columns.Add("ClubID", typeof(Int16), null);
            dtPreference.Columns.Add("MembershipID", typeof(string), null);
            dtPreference.Columns.Add("IsDeleted", typeof(string), null);

            DataTable dtMedia = new DataTable("Media");
            dtMedia.Columns.Add("MediaID", typeof(Int16), null);
            dtMedia.Columns.Add("MediaDescription", typeof(string), null);
            objClubs = preferenceserviceClient.ViewClubDetails(customerID);
            if (objClubs != null && objClubs.ClubInformation.Count > 0)
            {
                for (int i = 0; i < objClubs.ClubInformation.Count; i++)
                {
                    if (objClubs.ClubInformation[i].ClubID == 1 && objClubs.ClubInformation[i].DateOfBirth != "" && objClubs.ClubInformation[i].IsDeleted == "N")
                    {
                        if (dtPreference.Rows.Count < 5)
                        {
                            dtPreference.Rows.Add("", objClubs.ClubInformation[i].DateOfBirth, objClubs.ClubInformation[i].Media, objClubs.ClubInformation[i].ClubID,
                                objClubs.ClubInformation[i].MembershipID, objClubs.ClubInformation[i].IsDeleted);
                        }
                    }
                    //CCMCA-4300
                    if (objClubs.ClubInformation[i].ClubID == 4 && objClubs.ClubInformation[i].IsDeleted == "N" && chkAviosStandard.Checked && isAviosMembershipEnable)
                    {
                        txtAviosMembership.Text = objClubs.ClubInformation[i].MembershipID;
                        txtBAvios.Text = "";
                        txtVirgnMembershipID.Text = "";


                    }
                    else if (objClubs.ClubInformation[i].ClubID == 4 && objClubs.ClubInformation[i].IsDeleted == "N" && chkAviosPremium.Checked && isAviosMembershipEnable)
                    {
                        txtAviosMembership.Text = objClubs.ClubInformation[i].MembershipID;
                        txtBAvios.Text = "";
                        txtVirgnMembershipID.Text = "";


                    }
                    //End
                    else if (objClubs.ClubInformation[i].ClubID == 3 && objClubs.ClubInformation[i].IsDeleted == "N" && chkBAviosStandard.Checked)
                    {
                        txtBAvios.Text = objClubs.ClubInformation[i].MembershipID;
                        txtVirgnMembershipID.Text = "";
                        txtAviosMembership.Text = "";


                    }
                    else if (objClubs.ClubInformation[i].ClubID == 3 && objClubs.ClubInformation[i].IsDeleted == "N" && chkBAviosPremium.Checked)
                    {
                        txtBAvios.Text = objClubs.ClubInformation[i].MembershipID;
                        txtVirgnMembershipID.Text = "";
                        txtAviosMembership.Text = "";


                    }
                    else if (objClubs.ClubInformation[i].ClubID == 2 && objClubs.ClubInformation[i].IsDeleted == "N" && chkVirginAtlantic.Checked)
                    {
                        txtVirgnMembershipID.Text = objClubs.ClubInformation[i].MembershipID;
                        txtBAvios.Text = "";
                        txtAviosMembership.Text = "";

                    }

                }
            }

        }

        public bool ValidatePree()
        {
            try
            {
                yourchangessaved.Visible = false;
                bool berrorFlag = true;
                //string regEmail = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})$";
                //string regMobile = "^[0-9]*$";
                //string regDate = @"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$";
                string regEmail = hdnemailreg.Value;
                string regMobile = hdnphonenumberreg.Value;
                string regDate = hdndatereg.Value;
                //lclChangesSaved.Text = "";
                txtBAvios.CssClass = "";
                txtAviosMembership.CssClass = "";
                errMessageBTemail = string.Empty;
                errMessageBTMobile = string.Empty;
                txtBTMobile.CssClass = "";
                txtBTEmail.CssClass = "";
                txtVirgnMembershipID.CssClass = "";
                errMessageClubcardStatement = string.Empty;
                errMessageAviosMembership = string.Empty;
                //CCMCA-4700
                if (chkAviosStandard.Checked == true && isAviosMembershipEnable)
                {

                    txtBAvios.Text = "";
                    txtVirgnMembershipID.Text = "";
                    if (!Helper.ValidateAviosMembershipNumber(this.txtAviosMembership.Text.Trim(), regMobile, aviosLength))
                    {
                        spnAviosMembership.Attributes.Add("style", "display:block");
                        errMessageAviosMembership = GetLocalResourceObject("ValidMembership.Text").ToString();//"Please enter a valid membership ID";
                        txtAviosMembership.CssClass = "errorFld";
                        berrorFlag = false;
                        txtAviosMembership.Focus();
                        //txtBAvios.CssClass = "";

                    }
                }
                if (chkAviosPremium.Checked == true && isAviosMembershipEnable)
                {
                    txtBAvios.Text = "";
                    txtVirgnMembershipID.Text = "";
                    if (!Helper.ValidateAviosMembershipNumber(this.txtAviosMembership.Text.Trim(), regMobile, aviosLength))
                    {
                        spnAviosMembership.Attributes.Add("style", "display:block");
                        errMessageAviosMembership = GetLocalResourceObject("ValidMembership.Text").ToString();//"Please enter a valid membership ID";
                        txtAviosMembership.CssClass = "errorFld";
                        berrorFlag = false;
                        txtAviosMembership.Focus();
                        //txtBAvios.CssClass = "";

                    }

                }
                //End
                if (chkBAviosStandard.Checked == true)
                {
                    txtAviosMembership.Text = "";
                    txtVirgnMembershipID.Text = "";
                    if (!Helper.ValidateBAAviosMembership(this.txtBAvios.Text.Trim(), regMobile, baAviosLength))
                    {
                        spanBA.Attributes.Add("style", "display:block");
                        errMessageClubcardStatement = GetLocalResourceObject("ValidMembership.Text").ToString();//"Please enter a valid membership ID";
                        txtBAvios.CssClass = "errorFld";
                        berrorFlag = false;
                        txtBAvios.Focus();
                        //txtBAvios.CssClass = "";
                    }
                }


                if (chkVirginAtlantic.Checked == true)
                {
                    txtAviosMembership.Text = "";
                    txtBAvios.Text = "";
                    if (!Helper.ValidateVirginMembershipNumber(this.txtVirgnMembershipID.Text.Trim(), this.txtVirgnMembershipID.Text.Length.ToString(), regMobile, virginALength))
                    {
                        spanVirgin.Attributes.Add("style", "display:block");
                        errMessageClubcardStatementVirgin = GetLocalResourceObject("ValidMembership.Text").ToString();//"Please enter a valid membership ID";
                        txtVirgnMembershipID.CssClass = "errorFld";
                        berrorFlag = false;
                        txtVirgnMembershipID.Focus();
                        // txtVirgnMembershipID.CssClass = "";
                    }
                }
                else
                    txtVirgnMembershipID.CssClass = "";

                if (chkBAviosPremium.Checked == true)
                {
                    txtVirgnMembershipID.Text = "";
                    txtAviosMembership.Text = "";
                    //errMessageClubcardStatementPre
                    if (!Helper.IsRegexMatch(this.txtBAvios.Text.Trim(), regMobile, false, false))
                    {
                        spanBA.Attributes.Add("style", "display:block");
                        errMessageClubcardStatement = GetLocalResourceObject("ValidMembership.Text").ToString();//"Please enter a valid membership ID";
                        txtBAvios.CssClass = "errorFld";
                        berrorFlag = false;
                        txtBAvios.Focus();
                        // txtVirgnMembershipID.CssClass = "";
                    }

                    else if (txtBAvios.Text.Length != txtBAvios.MaxLength)
                    {
                        spanBA.Attributes.Add("style", "display:block");
                        errMessageClubcardStatement = GetLocalResourceObject("ValidMembership.Text").ToString();// "Please enter a valid membership ID";
                        txtBAvios.CssClass = "errorFld";
                        berrorFlag = false;
                        txtBAvios.Focus();
                    }

                }
                //Main Customer
                if (chkBabyTodlerOptIn.Checked)
                {
                    divBTOptIn.Disabled = false;
                    divBTOptIn.Attributes["class"] = "";
                    hdnBTCheck.Value = "1";
                    if (grdBabyTodlerOptIn.Rows.Count <= 0)
                    {
                        spangrdOptIn.Attributes.Add("style", "display:block");
                        errMessage = GetLocalResourceObject("ValidDateFormat.Text").ToString(); //"Please add a child with valid date of birth in DD/MM/YYYY format.";
                        berrorFlag = false;
                    }
                    else
                    {
                        for (int i = 0; i < grdBabyTodlerOptIn.Rows.Count; i++)
                        {
                            TextBox txtDOB = (TextBox)grdBabyTodlerOptIn.Rows[i].Cells[1].FindControl("txtDOB");
                            if (!Helper.IsRegexMatch(txtDOB.Text.Trim(), regDate, false, false))
                            {
                                spangrdOptIn.Attributes.Add("style", "display:block");
                                errMessage = GetLocalResourceObject("ValidDateFormat.Text").ToString();//"Please enter a valid date of birth in DD/MM/YYYY format.";
                                txtDOB.CssClass = "errorFld";
                                berrorFlag = false;
                                txtDOB.Focus();
                            }
                            else
                            {
                                DateTime dDate = Convert.ToDateTime(txtDOB.Text.ToString().Trim());
                                TimeSpan span = DateTime.Now - dDate;
                                if (span.TotalDays > 0)
                                {
                                    if (span.TotalDays > Convert.ToInt32(hdnConfigDOB.Value))
                                    {
                                        spangrdOptIn.Attributes.Add("style", "display:block");
                                        errMessage = GetLocalResourceObject("Validdateless.Text").ToString() + lbltotalChild.Text + GetLocalResourceObject("FormatHaif.Text").ToString();
                                        txtDOB.CssClass = "errorFld";
                                        berrorFlag = false;
                                        txtDOB.Focus();
                                    }
                                }
                                else
                                {
                                    if (span.TotalDays * (-1) > 275)
                                    {
                                        spangrdOptIn.Attributes.Add("style", "display:block");
                                        errMessage = GetLocalResourceObject("removeentry.Text").ToString();
                                        txtDOB.CssClass = "errorFld";
                                        berrorFlag = false;
                                        txtDOB.Focus();
                                    }
                                }
                            }
                        }

                    }
                }
                else if (liBabyTodlerOptOut.Visible && !chkBabyTodlerOptOut.Checked)
                {
                    if (grdBabyTodlerOptOut.Rows.Count <= 0)
                    {
                        chkBabyTodlerOptOut.Checked = true;
                    }
                    else
                    {
                        for (int i = 0; i < grdBabyTodlerOptOut.Rows.Count; i++)
                        {
                            TextBox txtDOB = (TextBox)grdBabyTodlerOptOut.Rows[i].Cells[1].FindControl("txtDOB");
                            if (!Helper.IsRegexMatch(txtDOB.Text.Trim(), regDate, false, false))
                            {
                                spangrdOptOut.Attributes.Add("style", "display:block");
                                errMessage = GetLocalResourceObject("crtformat.Text").ToString();
                                txtDOB.CssClass = "errorFld";
                                berrorFlag = false;
                                txtDOB.Focus();
                            }
                            else
                            {
                                DateTime dDate = Convert.ToDateTime(txtDOB.Text.ToString().Trim());
                                TimeSpan span = DateTime.Now - dDate;
                                if (span.TotalDays > 0)
                                {
                                    if (span.TotalDays > Convert.ToInt32(hdnConfigDOB.Value))
                                    {
                                        spangrdOptOut.Attributes.Add("style", "display:block");
                                        errMessage = GetLocalResourceObject("Validdateless.Text").ToString() + lbltotalChild.Text + GetLocalResourceObject("FormatHaif.Text").ToString(); //" years in DD/MM/YYYY format or remove the child entry.";
                                        txtDOB.CssClass = "errorFld";
                                        berrorFlag = false;
                                        txtDOB.Focus();
                                    }
                                }
                                else
                                {
                                    if (span.TotalDays * (-1) > 275)
                                    {
                                        spangrdOptOut.Attributes.Add("style", "display:block");
                                        errMessage = GetLocalResourceObject("removeentry.Text").ToString();//"Please enter a valid due date less than 9 months in DD/MM/YYYY format or remove the child entry.";
                                        txtDOB.CssClass = "errorFld";
                                        berrorFlag = false;
                                        txtDOB.Focus();
                                    }
                                }
                            }
                        }

                    }
                }
                if (chkBabyTodlerOptIn.Checked)
                {

                    grdBabyTodlerOptIn.Enabled = true;
                    divBTOptIn.Attributes["class"] = "";
                    divOptInBT.Attributes.Add("style", "display:block");
                    divOptInBT.Attributes.Add("style", "width:100%");
                }
                else
                {
                    divOptInBT.Attributes.Add("style", "display:none");
                }
                //Associate Customer B & T

                if (chkAssoBabyTodlerOptIn.Checked)
                {
                    divAssoOptInBT.Disabled = false;
                    divAssoOptInBT.Attributes["class"] = "";
                    hdnBTAssoCheck.Value = "1";
                    if (grdBabyTodlerAssoOptIn.Rows.Count <= 0)
                    {
                        spangrdOptInAsso.Attributes.Add("style", "display:block");
                        errAssoMessage = GetLocalResourceObject("ValidDateFormat.Text").ToString();//"Please add a child with valid date of birth in DD/MM/YYYY format.";
                        berrorFlag = false;
                    }
                    else
                    {
                        for (int i = 0; i < grdBabyTodlerAssoOptIn.Rows.Count; i++)
                        {
                            TextBox txtDOB = (TextBox)grdBabyTodlerAssoOptIn.Rows[i].Cells[1].FindControl("txtDOB");
                            if (!Helper.IsRegexMatch(txtDOB.Text.Trim(), regDate, false, false))
                            {
                                spangrdOptInAsso.Attributes.Add("style", "display:block");
                                errAssoMessage = GetLocalResourceObject("crtformat.Text").ToString();//"Please enter a valid date of birth in DD/MM/YYYY format.";
                                txtDOB.CssClass = "errorFld";
                                berrorFlag = false;
                                txtDOB.Focus();
                            }
                            else
                            {
                                DateTime dDate = Convert.ToDateTime(txtDOB.Text.ToString().Trim());
                                TimeSpan span = DateTime.Now - dDate;
                                if (span.TotalDays > 0)
                                {
                                    if (span.TotalDays > Convert.ToInt32(hdnConfigDOB.Value))
                                    {
                                        spangrdOptInAsso.Attributes.Add("style", "display:block");
                                        //errAssoMessage = "Please enter a valid date of birth less than " + lbltotalChild.Text + " years in DD/MM/YYYY format or remove the child entry.";
                                        errAssoMessage = GetLocalResourceObject("Validdateless.Text").ToString() + lbltotalChild.Text + GetLocalResourceObject("FormatHaif.Text").ToString();
                                        txtDOB.CssClass = "errorFld";
                                        berrorFlag = false;
                                        txtDOB.Focus();
                                    }
                                }
                                else
                                {
                                    if (span.TotalDays * (-1) > 275)
                                    {
                                        spangrdOptInAsso.Attributes.Add("style", "display:block");
                                        //errAssoMessage = "Please enter a valid due date less than 9 months in DD/MM/YYYY format or remove the child entry.";
                                        errAssoMessage = GetLocalResourceObject("removeentry.Text").ToString();
                                        txtDOB.CssClass = "errorFld";
                                        berrorFlag = false;
                                        txtDOB.Focus();
                                    }
                                }
                            }
                        }

                    }
                }
                else if (liAssoBabyTodlerOptOut.Visible && !chkAssoBabyTodlerOptOut.Checked)
                {
                    if (grdBabyTodlerAssoOptOut.Rows.Count <= 0)
                    {
                        chkAssoBabyTodlerOptOut.Checked = true;
                    }
                    else
                    {
                        for (int i = 0; i < grdBabyTodlerAssoOptOut.Rows.Count; i++)
                        {
                            TextBox txtDOB = (TextBox)grdBabyTodlerAssoOptOut.Rows[i].Cells[1].FindControl("txtDOB");
                            if (!Helper.IsRegexMatch(txtDOB.Text.Trim(), regDate, false, false))
                            {
                                spangrdOptOutAsso.Attributes.Add("style", "display:block");
                                errAssoMessage = GetLocalResourceObject("crtformat.Text").ToString(); //"Please enter a valid date of birth in DD/MM/YYYY format.";
                                txtDOB.CssClass = "errorFld";
                                berrorFlag = false;
                                txtDOB.Focus();
                            }
                            else
                            {
                                DateTime dDate = Convert.ToDateTime(txtDOB.Text.ToString().Trim());
                                TimeSpan span = DateTime.Now - dDate;
                                if (span.TotalDays > 0)
                                {
                                    if (span.TotalDays > Convert.ToInt32(hdnConfigDOB.Value))
                                    {
                                        spangrdOptOutAsso.Attributes.Add("style", "display:block");
                                        //errAssoMessage = "Please enter a valid date of birth less than " + lbltotalChild.Text + " years in DD/MM/YYYY format or remove the child entry.";
                                        errAssoMessage = GetLocalResourceObject("Validdateless.Text").ToString() + lbltotalChild.Text + GetLocalResourceObject("FormatHaif.Text").ToString();
                                        txtDOB.CssClass = "errorFld";
                                        berrorFlag = false;
                                        txtDOB.Focus();
                                    }
                                }
                                else
                                {
                                    if (span.TotalDays * (-1) > 275)
                                    {
                                        spangrdOptOutAsso.Attributes.Add("style", "display:block");
                                        errAssoMessage = GetLocalResourceObject("removeentry.Text").ToString();//"Please enter a valid due date less than 9 months in DD/MM/YYYY format or remove the child entry.";
                                        txtDOB.CssClass = "errorFld";
                                        berrorFlag = false;
                                        txtDOB.Focus();
                                    }
                                }
                            }
                        }

                    }
                }
                if (chkAssoBabyTodlerOptIn.Checked)
                {

                    grdBabyTodlerAssoOptIn.Enabled = true;
                    divAssoOptInBT.Attributes["class"] = "";
                    divOptInBTAsso.Attributes.Add("style", "display:block");
                    divOptInBTAsso.Attributes.Add("style", "width:100%");
                }
                else
                {
                    divOptInBTAsso.Attributes.Add("style", "display:none");
                }
                if (radioEmail.Checked && !txtBTEmail.ReadOnly)
                {
                    if (!Helper.IsRegexMatch(this.txtBTEmail.Text.Trim(), regEmail, false, false))
                    {
                        spanBTEmail.Attributes.Add("style", "display: block;width:300px");
                        errMessageBTemail = GetLocalResourceObject("ValidEmailAdd.Text").ToString();//"Please enter a valid email address";//ValidEmailAdd.Text
                        txtBTEmail.CssClass = "errorFld";
                        berrorFlag = false;
                        txtBTEmail.Focus();
                    }
                }
                if (radioSMS.Checked && !txtBTMobile.ReadOnly)
                {
                    if (!Helper.IsRegexMatch(this.txtBTMobile.Text.Trim(), regMobile, true, false))
                    {
                        spanBTMobile.Attributes.Add("style", "display: block;width:300px");
                        this.errMessageBTMobile = GetLocalResourceObject("ValidMobNum.Text").ToString();//"Please enter a valid mobile phone number";
                        txtBTMobile.CssClass = "errorFld";
                        berrorFlag = false;
                        txtBTMobile.Focus();
                    }

                    else if (txtBTMobile.Text.Trim() != "")
                    {
                        if ((!string.IsNullOrEmpty(hdnMobileNoMinVal.Value))
                    && txtBTMobile.Text.Trim().Length < Convert.ToInt16(hdnMobileNoMinVal.Value.Trim()))
                        {
                            spanBTMobile.Attributes.Add("style", "display: block;width:300px");
                            this.errMessageBTMobile = GetLocalResourceObject("ValidMobNum.Text").ToString();//"Please enter a valid mobile phone number";
                            txtBTMobile.CssClass = "errorFld";
                            berrorFlag = false;
                            txtBTMobile.Focus();
                        }
                        else if (hdnMobileNoPrefix.Value.Contains(","))
                        {
                            string[] mobPrefixes = hdnMobileNoPrefix.Value.Split(',');
                            bool flgMobPrefix = false;

                            for (int i = 0; i < mobPrefixes.Length; i++)
                            {
                                if (txtBTMobile.Text.Trim().Substring(0, mobPrefixes[i].Trim().Length) == mobPrefixes[i].ToString())
                                {
                                    flgMobPrefix = true;
                                    break;
                                }
                            }

                            if (!flgMobPrefix)
                            {
                                spanBTMobile.Attributes.Add("style", "display: block;width:300px");
                                this.errMessageBTMobile = GetLocalResourceObject("ValidMobNum.Text").ToString(); //"Please enter a valid mobile phone number";
                                txtBTMobile.CssClass = "errorFld";
                                berrorFlag = false;
                                txtBTMobile.Focus();
                            }
                        }
                        else if (txtBTMobile.Text.Trim().Substring(0, hdnMobileNoPrefix.Value.Trim().Length) != hdnMobileNoPrefix.Value)
                        {
                            spanBTMobile.Attributes.Add("style", "display: block;width:300px");
                            this.errMessageBTMobile = GetLocalResourceObject("ValidMobNum.Text").ToString(); //"Please enter a valid mobile phone number";
                            txtBTMobile.CssClass = "errorFld";
                            berrorFlag = false;
                            txtBTMobile.Focus();
                        }
                    }
                }
                return berrorFlag;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
            }
        }


        /// <summary>
        /// LoadConfigDetails to load configuration values like prefix and max/min length of mobile field and also defaultconfig details from
        /// Configuration table Configuartion type 3,9,5
        /// </summary>
        public void LoadConfigDetails()
        {
            string mobileNoMinValue = string.Empty;
            string mobileNoMaxValue = string.Empty;
            int rowCount;

            resultXml = string.Empty;
            conditionXML = "3,9,5,21,27,10";
            errorXml = string.Empty;
            rowCount = 0;
            serviceClient = new CustomerServiceClient();

            if (serviceClient.GetConfigDetails(out errorXml, out resultXml, out rowCount, conditionXML, culture))
            {
                resulDoc = new XmlDocument();
                resulDoc.LoadXml(resultXml);
                dsConfigDetails.ReadXml(new XmlNodeReader(resulDoc));
                if (dsConfigDetails.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                    {
                        if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber")
                        {
                            mobileNoMinValue = dr["ConfigurationValue1"].ToString();
                            mobileNoMaxValue = dr["ConfigurationValue2"].ToString();

                            //If Max value is not configured in the table, then assign DB field length as Max length
                            if (!string.IsNullOrEmpty(mobileNoMaxValue))
                            {
                                txtBTMobile.Attributes.Add("MaxLength", mobileNoMaxValue);
                            }
                            hdnMobileNoMinVal.Value = mobileNoMinValue.ToString();
                        }
                        //For Mobile phone number prefix
                        else if (dr["ConfigurationType"].ToString().Trim() == "9" && dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber")
                        {
                            hdnMobileNoPrefix.Value = dr["ConfigurationValue1"].ToString();
                        }
                        else if (dr["ConfigurationType"].ToString().Trim() == "3")
                        {
                            sdefaultpreferenceType = dr["ConfigurationName"].ToString().Trim();
                        }
                        else if (dr["ConfigurationType"].ToString().Trim() == "21")
                        {
                            hdnConfigDOB.Value = dr["ConfigurationValue1"].ToString();
                        }

                        else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "EmailAddress")
                        {
                            hdnemailreg.Value = dr["ConfigurationValue1"].ToString();
                        }
                        else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Date")
                        {
                            hdndatereg.Value = dr["ConfigurationValue1"].ToString();
                        }
                        else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "PhoneNumber")
                        {
                            hdnphonenumberreg.Value = dr["ConfigurationValue1"].ToString();
                        }
                        // R1.6 send mail functionality
                        else if (dr["ConfigurationType"].ToString().Trim() == "27")
                        {
                            if (dr["ConfigurationName"].ToString().Trim() == "43")
                            {
                                hdnSendEmailForEmail.Value = dr["ConfigurationValue1"].ToString().Trim();
                            }
                            else if (dr["ConfigurationName"].ToString().Trim() == "44")
                            {
                                hdnSendEmailForSMS.Value = dr["ConfigurationValue1"].ToString().Trim();
                            }
                            else if (dr["ConfigurationName"].ToString().Trim() == "45")
                            {
                                hdnSendEmailForPost.Value = dr["ConfigurationValue1"].ToString().Trim();
                            }
                            else if (dr["ConfigurationName"].ToString().Trim() == "17")
                            {
                                hdnSendEmailForVirgin.Value = dr["ConfigurationValue1"].ToString().Trim();
                            }
                            else if (dr["ConfigurationName"].ToString().Trim() == "10" || dr["ConfigurationName"].ToString().Trim() == "14")
                            {
                                hdnSendEmailForBAAvios.Value = dr["ConfigurationValue1"].ToString().Trim();
                            }
                            else if (dr["ConfigurationName"].ToString().Trim() == "11" || dr["ConfigurationName"].ToString().Trim() == "12")
                            {
                                hdnSendEmailForAvios.Value = dr["ConfigurationValue1"].ToString().Trim();
                            }
                            else if (dr["ConfigurationName"].ToString().Trim() == "15")
                            {
                                hdnSendEmailForEcoupon.Value = dr["ConfigurationValue1"].ToString().Trim();
                            }
                            else if (dr["ConfigurationName"].ToString().Trim() == "16")
                            {
                                hdnSendEmailForSaveTrees.Value = dr["ConfigurationValue1"].ToString().Trim();
                            }
                            else if (dr["ConfigurationName"].ToString().Trim() == "48")
                            {
                                hdnSendEmailForBandT.Value = dr["ConfigurationValue1"].ToString().Trim();
                            }
                            else if (dr["ConfigurationName"].ToString().Trim() == "13")
                            {
                                hdnSendEmailForChristmasSaver.Value = dr["ConfigurationValue1"].ToString().Trim();
                            }
                            else if (DataProtectionTable.Visible)
                            {
                                if (dr["ConfigurationName"].ToString().Trim() == "27")
                                {
                                    hdnSendMailForDP27.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "28")
                                {
                                    hdnSendMailForDP28.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "29")
                                {
                                    hdnSendMailForDP29.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "30")
                                {
                                    hdnSendMailForDP30.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "31")
                                {
                                    hdnSendMailForDP31.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "32")
                                {
                                    hdnSendMailForDP32.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "33")
                                {
                                    hdnSendMailForDP33.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "34")
                                {
                                    hdnSendMailForDP34.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "35")
                                {
                                    hdnSendMailForDP35.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "36")
                                {
                                    hdnSendMailForDP36.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "37")
                                {
                                    hdnSendMailForDP37.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "38")
                                {
                                    hdnSendMailForDP38.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                //LCM changes

                                else if (dr["ConfigurationName"].ToString().Trim() == "50")
                                {
                                    hdnSendMailForDP39.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "51")
                                {
                                    hdnSendMailForDP40.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "52")
                                {
                                    hdnSendMailForDP41.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "53")
                                {
                                    hdnSendMailForDP42.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                //LCM changes

                            }
                            else if (!DataProtectionTable.Visible)
                            {
                                if (dr["ConfigurationName"].ToString().Trim() == "6")
                                {
                                    hdnSendMailForDP6.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "7")
                                {
                                    hdnSendMailForDP7.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "8")
                                {
                                    hdnSendMailForDP8.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                //LCM Changes
                                else if (dr["ConfigurationName"].ToString().Trim() == "49")
                                {
                                    hdnSendMailForDPBonus.Value = dr["ConfigurationValue1"].ToString().Trim();
                                }
                                //LCM Changes
                            }
                        }
                    }
                }
            }
        }
        protected void LoadBnTPreferences(long CustomerID, int rowNumber)
        {
            try
            {
                preferenceserviceClient = new PreferenceServiceClient();
                CustomerPreference objPreference = new CustomerPreference();
                objPreference = preferenceserviceClient.ViewCustomerPreference(CustomerID, PreferenceType.NULL, true);

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

                    // To load Preferences enabled for the country
                    string sprefType = string.Empty;
                    List<string> liPreferenceTypes = new List<string>();

                    for (int i = 0; i < objPreference.Preference.Count; i++)
                    {
                        sprefType = objPreference.Preference[i].PreferenceID.ToString();
                        liPreferenceTypes.Add(sprefType);
                    }

                    if (liPreferenceTypes.Contains(BusinessConstants.BABYTODLER_CLUB.ToString()))
                    {
                        hdnIsBnT.Value = "false";
                        BntHeader.Visible = false;
                        if (PreferenceIds.Contains(BusinessConstants.BABYTODLER_CLUB.ToString()))
                        {


                            if (rowNumber == 0)
                            {
                                liBabyTodlerOptIn.Visible = false;
                                liBabyTodlerOptOut.Visible = false;
                                LoadBabtTodlerDetails();
                            }
                            else if (rowNumber == 1)
                            {
                                liAssoBabyTodlerOptIn.Visible = false;
                                liAssoBabyTodlerOptOut.Visible = false;
                                LoadAssoBabtTodlerDetails();
                            }
                        }
                        else
                        {


                            if (rowNumber == 0)
                            {
                                liBabyTodlerOptIn.Visible = false;
                                liBabyTodlerOptOut.Visible = false;
                                LoadBabtTodlerDetails();
                            }
                            else if (rowNumber == 1)
                            {
                                liAssoBabyTodlerOptIn.Visible = false;
                                liAssoBabyTodlerOptOut.Visible = false;
                                LoadAssoBabtTodlerDetails();
                            }
                        }

                        //bpreferenceFlag = true;
                    }
                    else
                    {
                        BntHeader.Visible = false;
                        hdnIsBnT.Value = "false";
                        divBnTAsso.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Preferences.LoadBnTPreferences() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Preferences.LoadBnTPreferences() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Preferences.LoadBnTPreferences()");
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
        protected void LoadPreferences()
        {
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Preferences.LoadPreferences()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Preferences.LoadPreferences()");
                #endregion
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID")))
                {
                    preferenceserviceClient = new PreferenceServiceClient();
                    customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                    Label lblXmasSaver = (Label)Master.FindControl("lblXmasSaver");
                    ContentPlaceHolder leftNav = (ContentPlaceHolder)Master.FindControl("CustDetailsLeftNav");
                    Label lblCustType = (Label)leftNav.FindControl("lblCustType");
                    //chkAviosPremium.Checked = false;
                    chkAviosStandard.Checked = false;
                    //chkBAviosPremium.Checked = false;
                    chkBAviosStandard.Checked = false;
                    chkVirginAtlantic.Checked = false;
                    //LCM changes
                    chkBonusCoupon.Checked = false;
                    //LCM changes
                    preferenceserviceClient = new PreferenceServiceClient();
                    customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                    //Store customerID in hidden variable to use for Site Catalyst
                    //bool bpreferenceFlag = false; // To hide the section if no preference in the section in Isdeleted='N' state
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

                        // To load Preferences enabled for the country
                        string sprefType = string.Empty;
                        List<string> liPreferenceTypes = new List<string>();

                        for (int i = 0; i < objPreference.Preference.Count; i++)
                        {
                            sprefType = objPreference.Preference[i].PreferenceID.ToString();
                            liPreferenceTypes.Add(sprefType);
                        }

                        //LCM changes

                        if (!((liPreferenceTypes.Contains(BusinessConstants.BONUSCOUPON_MAIL.ToString())) && (liPreferenceTypes.Contains(BusinessConstants.BONUSCOUPON_EMAIL.ToString())) && (liPreferenceTypes.Contains(BusinessConstants.BONUSCOUPON_PHONE.ToString())) && (liPreferenceTypes.Contains(BusinessConstants.BONUSCOUPON_SMS.ToString())))) //Tesco mailing
                        {
                            hdnLCMPre.Value = "true";
                        }

                        if (!liPreferenceTypes.Contains(BusinessConstants.BONUSCOUPON_BYMAIL.ToString()))
                        {
                            liBonusCoupon.Visible = false;
                        }
                        else
                        {
                            if (PreferenceIds.Contains(BusinessConstants.BONUSCOUPON_BYMAIL.ToString()))
                            {
                                chkBonusCoupon.Checked = isOptInBehavior == true ? true : false;
                                hdnPrefBonus.Value = "true";
                            }
                            else
                            {
                                chkBonusCoupon.Checked = isOptInBehavior == false ? true : false;
                                hdnPrefBonus.Value = "true";
                            }

                        }
                        if (!liPreferenceTypes.Contains(BusinessConstants.BONUSCOUPON_BYMAIL.ToString()))
                        {
                            liBonusCoupon.Visible = false;
                        }
                        //LCM changes
                        if (!liPreferenceTypes.Contains(BusinessConstants.NONTESCOPROPMATION_BYMAIL.ToString())) //Non-tesco mailing
                        {
                            liPartnerinfo.Visible = false;
                        }
                        else
                        {
                            if (PreferenceIds.Contains(BusinessConstants.NONTESCOPROPMATION_BYMAIL.ToString())) //Non-tesco mailing
                            {
                                //chkRecvPartnersOffrnInfo.Checked = true;
                                chkRecvPartnersOffrnInfo.Checked = isOptInBehavior == true ? true : false;
                                hdnPref7.Value = "true";
                                //bpreferenceFlag = true;
                            }
                            else
                            {
                                chkRecvPartnersOffrnInfo.Checked = isOptInBehavior == false ? true : false;
                                hdnPref7.Value = "true";

                            }
                        }
                        if (!liPreferenceTypes.Contains(BusinessConstants.RESEARCH_BYPHONE.ToString()))
                        {
                            liResearch.Visible = false;
                        }
                        else
                        {
                            if (PreferenceIds.Contains(BusinessConstants.RESEARCH_BYPHONE.ToString()))
                            {
                                //chkDontContact.Checked = true;
                                chkDontContact.Checked = isOptInBehavior == true ? true : false;
                                hdnPref8.Value = "true";
                                //bpreferenceFlag = true;
                            }
                            else
                            {
                                chkDontContact.Checked = isOptInBehavior == false ? true : false;
                                hdnPref8.Value = "true";
                            }
                        }
                        if (!liPreferenceTypes.Contains(BusinessConstants.TESCOPROMOTION_BYMAIL.ToString())) //Tesco mailing
                        {
                            liTescoinfo.Visible = false;
                        }
                        else
                        {
                            if (PreferenceIds.Contains(BusinessConstants.TESCOPROMOTION_BYMAIL.ToString())) //Tesco mailing
                            {
                                //chkRecvTescoOffrnInfo.Checked = true;
                                chkRecvTescoOffrnInfo.Checked = isOptInBehavior == true ? true : false;
                                hdnPref6.Value = "true";
                                //bpreferenceFlag = true;
                            }
                            else
                            {
                                chkRecvTescoOffrnInfo.Checked = isOptInBehavior == false ? true : false;
                                hdnPref6.Value = "true";
                            }
                        }
                        if (!liPreferenceTypes.Contains(BusinessConstants.TESCO_GROUP_MAIL.ToString()))
                        {
                            DataProtectionTable.Visible = false;
                        }
                        else
                        {
                            //LCM changes
                            if (PreferenceIds.Contains(BusinessConstants.BONUSCOUPON_MAIL.ToString()))
                            {
                                hdnPref39.Value = "true";
                                chkBCMMail.Checked = true;
                            }
                            if (PreferenceIds.Contains(BusinessConstants.BONUSCOUPON_EMAIL.ToString()))
                            {
                                hdnPref40.Value = "true";
                                chkBCMEmail.Checked = true;
                            }
                            if (PreferenceIds.Contains(BusinessConstants.BONUSCOUPON_PHONE.ToString()))
                            {
                                hdnPref41.Value = "true";
                                chkBCMPhone.Checked = true;
                            }
                            if (PreferenceIds.Contains(BusinessConstants.BONUSCOUPON_SMS.ToString()))
                            {
                                hdnPref42.Value = "true";
                                chkBCMSms.Checked = true;
                            }

                            //LCM changes
                            DataProtectionTable.Visible = true;

                            if (DataProtectionTable.Visible == true)
                            {
                                if (hdnLCMPre.Value == "true")
                                {
                                    BCMPRE.Visible = false;
                                }
                            }
                        }
                        //Poland legal changes..:Start
                        if (isGridFormat)
                        {
                            if (PreferenceIds.Contains(BusinessConstants.TESCO_GROUP_MAIL.ToString()))
                            {
                                hdnPref27.Value = "true";
                                chkTGMail.Checked = true;
                            }
                            if (PreferenceIds.Contains(BusinessConstants.TESCO_GROUP_PHONE.ToString()))
                            {
                                hdnPref28.Value = "true";
                                chkTGPhone.Checked = true;
                            }
                            if (PreferenceIds.Contains(BusinessConstants.TESCO_GROUP_EMAIL.ToString()))
                            {
                                hdnPref29.Value = "true";
                                chkTGEMail.Checked = true;
                            }
                            if (PreferenceIds.Contains(BusinessConstants.TESCO_GROUP_SMS.ToString()))
                            {
                                hdnPref30.Value = "true";
                                chkTGSms.Checked = true;
                            }
                            if (PreferenceIds.Contains(BusinessConstants.TESCO_THIRD_PARTY_MAIL.ToString()))
                            {
                                hdnPref31.Value = "true";
                                chkTPMail.Checked = true;
                            }
                            if (PreferenceIds.Contains(BusinessConstants.TESCO_THIRD_PARTY_PHONE.ToString()))
                            {
                                hdnPref32.Value = "true";
                                chkTPPhone.Checked = true;
                            }
                            if (PreferenceIds.Contains(BusinessConstants.TESCO_THIRD_PARTY_EMAIL.ToString()))
                            {
                                hdnPref33.Value = "true";
                                chkTPEmail.Checked = true;
                            }
                            if (PreferenceIds.Contains(BusinessConstants.TESCO_THIRD_PARTY_SMS.ToString()))
                            {
                                hdnPref34.Value = "true";
                                chkTPSms.Checked = true;
                            }
                            if (PreferenceIds.Contains(BusinessConstants.RESEARCH_MAIL.ToString()))
                            {
                                hdnPref35.Value = "true";
                                chkRMail.Checked = true;
                            }
                            if (PreferenceIds.Contains(BusinessConstants.RESEARCH_PHONE.ToString()))
                            {
                                hdnPref36.Value = "true";
                                chkRphone.Checked = true;
                            }
                            if (PreferenceIds.Contains(BusinessConstants.RESEARCH_EMAIL.ToString()))
                            {
                                hdnPref37.Value = "true";
                                chkREmail.Checked = true;
                            }
                            if (PreferenceIds.Contains(BusinessConstants.RESEARCH_SMS.ToString()))
                            {
                                hdnPref38.Value = "true";
                                chkRSms.Checked = true;
                            }
                            DataProtectionTable.Visible = true;
                            //bpreferenceFlag = true;
                        }
                        else if (!isGridFormat)
                        {
                            DataProtectionTable.Visible = false;
                            if (liPreferenceTypes.Contains(BusinessConstants.TESCO_GROUP_MAIL.ToString()))
                            {
                                if (isOptInBehavior)
                                {
                                    if (PreferenceIds.Contains(BusinessConstants.TESCO_GROUP_MAIL.ToString()))
                                    {
                                        hdnPref27.Value = "true";
                                        hdnPref28.Value = "true";
                                        hdnPref29.Value = "true";
                                        hdnPref30.Value = "true";
                                        chkGrpTescoProducts.Checked = true;

                                    }
                                    liGrpTescoproducts.Visible = true;
                                }
                                else
                                {
                                    if (!PreferenceIds.Contains(BusinessConstants.TESCO_GROUP_MAIL.ToString()))
                                    {
                                        hdnPref27.Value = "true";
                                        hdnPref28.Value = "true";
                                        hdnPref29.Value = "true";
                                        hdnPref30.Value = "true";
                                        chkGrpTescoProducts.Checked = true;

                                    }
                                    liGrpTescoproducts.Visible = true;
                                }
                            }



                            if (liPreferenceTypes.Contains(BusinessConstants.TESCO_THIRD_PARTY_MAIL.ToString()))
                            {
                                if (isOptInBehavior)
                                {
                                    if (PreferenceIds.Contains(BusinessConstants.TESCO_THIRD_PARTY_MAIL.ToString()))
                                    {
                                        hdnPref31.Value = "true";
                                        hdnPref32.Value = "true";
                                        hdnPref33.Value = "true";
                                        hdnPref34.Value = "true";
                                        chkGrpPartnerOffers.Checked = true;
                                    }
                                    liGrpTescoOffer.Visible = true;
                                    //bpreferenceFlag = true;
                                }
                                else
                                {
                                    if (!PreferenceIds.Contains(BusinessConstants.TESCO_THIRD_PARTY_MAIL.ToString()))
                                    {
                                        hdnPref31.Value = "true";
                                        hdnPref32.Value = "true";
                                        hdnPref33.Value = "true";
                                        hdnPref34.Value = "true";
                                        chkGrpPartnerOffers.Checked = true;
                                    }
                                    liGrpTescoOffer.Visible = true;
                                    //bpreferenceFlag = true;
                                }
                            }
                            //}
                            //if (!isGridFormat)
                            //{
                            if (liPreferenceTypes.Contains(BusinessConstants.RESEARCH_MAIL.ToString()))
                            {
                                if (isOptInBehavior)
                                {
                                    if (PreferenceIds.Contains(BusinessConstants.RESEARCH_MAIL.ToString()))
                                    {
                                        hdnPref35.Value = "true";
                                        hdnPref36.Value = "true";
                                        hdnPref37.Value = "true";
                                        hdnPref38.Value = "true";
                                        chkGrpResearch.Checked = true;
                                    }
                                    liGrpTescoCustomerReasearch.Visible = true;
                                    //bpreferenceFlag = true;

                                }

                                else
                                {
                                    if (!PreferenceIds.Contains(BusinessConstants.RESEARCH_MAIL.ToString()))
                                    {
                                        hdnPref35.Value = "true";
                                        hdnPref36.Value = "true";
                                        hdnPref37.Value = "true";
                                        hdnPref38.Value = "true";
                                        chkGrpResearch.Checked = true;
                                    }
                                    liGrpTescoCustomerReasearch.Visible = true;
                                    //bpreferenceFlag = true;
                                }


                            }
                        }
                        //Ecoupon Issue--Live
                        if (liPreferenceTypes.Contains(BusinessConstants.ECOUPON.ToString()))
                        {
                            if (PreferenceIds.Contains(BusinessConstants.ECOUPON.ToString()))    //Ecoupon
                            {
                                chkEcoupon.Checked = true;
                            }
                        }
                        else
                        {
                            lbleCoupon.Visible = false;
                            diveCoupon.Visible = false;
                        }

                        if (liPreferenceTypes.Contains(BusinessConstants.SAVETREES.ToString()))
                        {
                            if (PreferenceIds.Contains(BusinessConstants.SAVETREES.ToString()))    //Save Trees
                            {
                                chkSaveTree.Checked = true;
                            }

                        }
                        else
                        {
                            lblSaveTree.Visible = false;
                            divSaveTree.Visible = false;
                        }

                        if (PreferenceIds.Contains(BusinessConstants.XMASSAVER.ToString()))    //Xmas Saver
                        {
                            chkXmasSaver.Checked = true;
                            lblXmasSaver.Visible = true;

                            //To change the customer type in left navigation bar
                            // lblCustType.Text = "Christmas saver";
                            lblCustType.Text = GetLocalResourceObject("CardTypeCS.Text").ToString();
                            Helper.SetTripleDESEncryptedCookie("lblCustType", "Christmas saver");
                            HtmlAnchor christmasSaver = (HtmlAnchor)Master.FindControl("christmasSaver");
                            christmasSaver.Visible = true;
                            hdnPref13.Value = "true";
                            //Clear earn rate after opt of the Airmiles/BAmiles.
                            //ltrAMilesEarnRate.Text = string.Empty;
                            //ltrBAMilesEarnRate.Text = string.Empty;
                        }
                        else if (PreferenceIds.Contains(BusinessConstants.AIRMILES_STD.ToString()))   //AirMiles Standard
                        {
                            //ltrAMilesEarnRate.Text = BusinessConstants.STANDARD_AMILES.ToString();
                            //To change the customer type in left navigation bar
                            //lblCustType.Text = "Airmiles";
                            lblCustType.Text = GetLocalResourceObject("CardTypeAM.Text").ToString();
                            Helper.SetTripleDESEncryptedCookie("lblCustType", "Airmiles");
                            lblXmasSaver.Visible = false;
                            lclAvios.Visible = true;
                            lclairpremium.Visible = false;
                            chkAviosStandard.Visible = true;
                            chkAviosStandard.Checked = true;
                            chkAviosPremium.Visible = false;
                            hdnPref12.Value = "true";
                            divAviosMemberShip.Visible = isAviosMembershipEnable ? true : false;
                        }
                        else if (PreferenceIds.Contains(BusinessConstants.AIRMILES_PREMIUM.ToString()))   //AirMiles Premium
                        {
                            //ltrAMilesEarnRate.Text = BusinessConstants.PRIMIUM_AMILES.ToString();
                            //To change the customer type in left navigation bar
                            //lblCustType.Text = "Airmiles";
                            lblCustType.Text = GetLocalResourceObject("CardTypeAM.Text").ToString();
                            Helper.SetTripleDESEncryptedCookie("lblCustType", "Airmiles");
                            lblXmasSaver.Visible = false;
                            //chkAviosPremium.Checked = true;
                            lclAvios.Visible = false;
                            lclairpremium.Visible = true;
                            chkAviosStandard.Visible = false;
                            chkAviosPremium.Visible = true;
                            chkAviosPremium.Checked = true;
                            hdnPref12.Value = "true";
                            divAviosMemberShip.Visible = isAviosMembershipEnable ? true : false;
                        }

                        else if (PreferenceIds.Contains(BusinessConstants.BAMILES_STD.ToString()))   //BA AirMiles Standard
                        {
                            //ltrBAMilesEarnRate.Text = BusinessConstants.STANDARD_BAMILES.ToString();

                            //To change the customer type in left navigation bar
                            //lblCustType.Text = "BA Miles";
                            lblCustType.Text = GetLocalResourceObject("CardTypeBAAM.Text").ToString();
                            Helper.SetTripleDESEncryptedCookie("lblCustType", "BA Miles");
                            lblXmasSaver.Visible = false;
                            lclBAvios.Visible = true;
                            lclBRPRE.Visible = false;
                            chkBAviosStandard.Visible = true;
                            chkBAviosStandard.Checked = true;
                            chkBAviosPremium.Visible = false;
                            hdnPref10.Value = "true";
                        }
                        else if (PreferenceIds.Contains(BusinessConstants.BAMILES_PREMIUM.ToString()))   //BA AirMiles Premium
                        {
                            //ltrBAMilesEarnRate.Text = BusinessConstants.PRIMIUM_BAMILES.ToString();

                            //To change the customer type in left navigation bar
                            //lblCustType.Text = "BA Miles";
                            lblCustType.Text = GetLocalResourceObject("CardTypeBAAM.Text").ToString();
                            Helper.SetTripleDESEncryptedCookie("lblCustType", "BA Miles");
                            lblXmasSaver.Visible = false;
                            //chkBAviosPremium.Checked = true;
                            lclBAvios.Visible = false;
                            lclBRPRE.Visible = true;
                            chkBAviosStandard.Visible = false;
                            chkBAviosPremium.Visible = true;
                            chkBAviosPremium.Checked = true;
                            hdnPref10.Value = "true";

                        }

                        else if (PreferenceIds.Contains(BusinessConstants.VIRGIN.ToString()))
                        {
                            chkVirginAtlantic.Checked = true;
                            hdnPref17.Value = "true";

                        }
                        else
                        {
                            //chkAirmiles.Checked = false;
                            //chkBAmiles.Checked = false;
                            //ltrAMilesEarnRate.Text = "";
                            //ltrBAMilesEarnRate.Text = "";

                            //To change the customer type in left navigation bar
                            //lblCustType.Text = "Standard";
                            lblCustType.Text = GetLocalResourceObject("CardTypeS.Text").ToString();
                            Helper.SetTripleDESEncryptedCookie("lblCustType", "Standard");
                            lblXmasSaver.Visible = false;
                        }
                        // if (liPreferenceTypes.Contains(BusinessConstants.BABYTODLER_CLUB.ToString()))
                        //{
                        //    if (PreferenceIds.Contains(BusinessConstants.BABYTODLER_CLUB.ToString()))
                        //    {
                        //        liBabyTodlerOptIn.Visible = false;
                        //        liAssoBabyTodlerOptIn.Visible = false;
                        //        liBabyTodlerOptOut.Visible = true;
                        //        liAssoBabyTodlerOptOut.Visible = true;
                        //        //LoadBabtTodlerDetails();
                        //        //LoadAssoBabtTodlerDetails();
                        //    }
                        //    else
                        //    {
                        //        liBabyTodlerOptIn.Visible = true;
                        //        liBabyTodlerOptOut.Visible = false;
                        //        liAssoBabyTodlerOptIn.Visible = true;
                        //        liAssoBabyTodlerOptOut.Visible = false;
                        //        //LoadBabtTodlerDetails();
                        //        //LoadAssoBabtTodlerDetails();
                        //    }

                        //    //bpreferenceFlag = true;
                        //}
                        if ((!(PreferenceIds.Contains(BusinessConstants.AIRMILES_PREMIUM.ToString()))) && (!(PreferenceIds.Contains(BusinessConstants.AIRMILES_STD.ToString()))))
                        {
                            lclAvios.Visible = true;
                            lclairpremium.Visible = false;
                            chkAviosStandard.Visible = true;
                            chkAviosPremium.Visible = false;
                        }

                        if ((!(PreferenceIds.Contains(BusinessConstants.BAMILES_PREMIUM.ToString()))) && (!(PreferenceIds.Contains(BusinessConstants.BAMILES_STD.ToString()))))
                        {
                            lclBAvios.Visible = true;
                            lclBRPRE.Visible = false;
                            chkBAviosStandard.Visible = true;
                            chkBAviosPremium.Visible = false;
                            txtBAvios.Text = "";
                        }
                        if (!(PreferenceIds.Contains(BusinessConstants.VIRGIN.ToString())))
                        {
                            txtVirgnMembershipID.Text = "";

                        }
                        if (!liPreferenceTypes.Contains(BusinessConstants.XMASSAVER.ToString()))    //Xmas Saver
                        {
                            lichristmasaver.Visible = false;
                        }
                        if (!liPreferenceTypes.Contains(BusinessConstants.AIRMILES_STD.ToString()))   //AirMiles Standard
                        {
                            liairMiles.Visible = false;
                        }
                        if (!liPreferenceTypes.Contains(BusinessConstants.BAMILES_STD.ToString()))   //BA AirMiles Standard
                        {
                            liBAairmile.Visible = false;
                        }
                        if (!liPreferenceTypes.Contains(BusinessConstants.VIRGIN.ToString()))   //Virgin Premium
                        {
                            liVirginMiles.Visible = false;
                        }
                        if (!liPreferenceTypes.Contains(BusinessConstants.BABYTODLER_CLUB.ToString()))
                        {
                            DivBT.Visible = false;
                        }

                        if (liPreferenceTypes.Contains(BusinessConstants.XMASSAVER.ToString()) || liPreferenceTypes.Contains(BusinessConstants.AIRMILES_STD.ToString()) || liPreferenceTypes.Contains(BusinessConstants.BAMILES_STD.ToString()) || liPreferenceTypes.Contains(BusinessConstants.VIRGIN.ToString()) || liPreferenceTypes.Contains(BusinessConstants.SAVETREES.ToString()))
                        {
                            DVForStmtPreferences.Visible = true;
                            ImageButton1.Visible = true;
                        }
                        else
                        {
                            DVForStmtPreferences.Visible = false;
                            ImageButton1.Visible = false;
                        }

                        if (liPreferenceTypes.Contains(BusinessConstants.POST_CONTACT.ToString()))    //Xmas Saver
                        {
                            if (PreferenceIds.Contains(BusinessConstants.POST_CONTACT.ToString()))    //Xmas Saver
                            {
                                radioPost.Checked = true;
                                hdnPref45.Value = "true";
                            }
                        }
                        else
                        {
                            liPost.Visible = false;
                        }
                        if (liPreferenceTypes.Contains(BusinessConstants.EMAIL_CONTACT.ToString()))    //Xmas Saver
                        {
                            if (PreferenceIds.Contains(BusinessConstants.EMAIL_CONTACT.ToString()))   //AirMiles Standard
                            {
                                radioEmail.Checked = true;
                                hdnPref43.Value = "true";
                            }
                        }
                        else
                        {
                            liEmail.Visible = false;
                        }
                        if (liPreferenceTypes.Contains(BusinessConstants.MOBILE_CONTACT.ToString()))    //Xmas Saver
                        {
                            if (PreferenceIds.Contains(BusinessConstants.MOBILE_CONTACT.ToString()))   //AirMiles Premium
                            {
                                radioSMS.Checked = true;
                                hdnPref44.Value = "true";
                            }
                        }
                        else
                        {
                            liSms.Visible = false;
                        }
                        if (liPreferenceTypes.Contains(BusinessConstants.BRAILLE_CONTACT.ToString()))    //Xmas Saver
                        {
                            if (PreferenceIds.Contains(BusinessConstants.BRAILLE_CONTACT.ToString()))   //AirMiles Premium
                            {
                                chkbraille.Checked = true;
                            }
                        }
                        else
                        {
                            liBraille.Visible = false;
                        }
                        if (liPreferenceTypes.Contains(BusinessConstants.LARGEPRINT_CONTACT.ToString()))    //Xmas Saver
                        {
                            if (PreferenceIds.Contains(BusinessConstants.LARGEPRINT_CONTACT.ToString()))   //AirMiles Premium
                            {
                                chkLP.Checked = true;
                            }
                        }
                        else
                        {
                            lilargeprint.Visible = false;
                        }
                        if (!((PreferenceIds.Contains(BusinessConstants.POST_CONTACT.ToString())) || (PreferenceIds.Contains(BusinessConstants.EMAIL_CONTACT.ToString()))
                                    || (PreferenceIds.Contains(BusinessConstants.MOBILE_CONTACT.ToString()))))
                        {
                            if (sdefaultpreferenceType == "43" && liEmail.Visible)
                            {
                                radioEmail.Checked = true;
                                radioPost.Checked = false;
                                radioSMS.Checked = false;
                            }
                            else if (sdefaultpreferenceType == "44" && liSms.Visible)
                            {
                                radioEmail.Checked = false;
                                radioPost.Checked = false;
                                radioSMS.Checked = true;
                            }
                            else if (sdefaultpreferenceType == "45" && liPost.Visible)
                            {
                                radioEmail.Checked = false;
                                radioPost.Checked = true;
                                radioSMS.Checked = false;
                            }
                        }

                        if (liPreferenceTypes.Contains(BusinessConstants.LARGEPRINT_CONTACT.ToString()) || liPreferenceTypes.Contains(BusinessConstants.BRAILLE_CONTACT.ToString()) || liPreferenceTypes.Contains(BusinessConstants.MOBILE_CONTACT.ToString()) || liPreferenceTypes.Contains(BusinessConstants.EMAIL_CONTACT.ToString()) || liPreferenceTypes.Contains(BusinessConstants.POST_CONTACT.ToString()))
                        {
                            divstatementPreferences.Visible = true;
                            //ImageButton1.Visible = true;
                        }
                        else
                        {
                            divstatementPreferences.Visible = false;
                            //ImageButton1.Visible = false;
                        }

                    }
                }
                else
                {
                    throw new Exception("Customer ID not available: " + customerID + "; errorXml: " + errorXml);
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Preferences.LoadPreferences()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Preferences.LoadPreferences()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.LoadPreferences() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.LoadPreferences() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.LoadPreferences()");
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




        /// <summary>
        /// Updates Clubcard Mailing preference and statement preferences.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateCustomerPreference(object sender, ImageClickEventArgs e)
        {
            string updateXml = string.Empty;
            string consumer = string.Empty;
            string amendDateTime = string.Empty;
            string dateFormat = string.Empty;

            try
            {
                dateFormat = ConfigurationManager.AppSettings["DateDisplayFormat"].ToString();
                //lclChangesSaved.Text = "";
                if (ValidatePree())
                {

                    btnConfirmPreferences.Enabled = false;
                    xmlCapability = new XmlDocument();
                    dsCapability = new DataSet();
                    for (int cust = 0; cust < Convert.ToInt32(hdnNoofCustomer.Value); cust++)
                    {
                        if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                        {
                            xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                            dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                            //Check if user has update role.
                            if (dsCapability.Tables[0].Columns.Contains("UpdateCustomerPreferences") != false)
                            {
                                ContentPlaceHolder leftNav = (ContentPlaceHolder)Master.FindControl("CustDetailsLeftNav");
                                Label lblCustType = (Label)leftNav.FindControl("lblCustType");
                                Label lblCustomerLastUpdatedBy = (Label)leftNav.FindControl("lblCustomerLastUpdatedBy");
                                ImageButton btnClicked = sender as ImageButton;

                                DataTable dtPreference = new DataTable("Preference");
                                dtPreference.Columns.Add("PreferenceID", typeof(Int16));
                                dtPreference.Columns.Add("OptStatus", typeof(Enum));
                                dtPreference.Columns.Add("updateDateTime", typeof(DateTime), null);
                                dtPreference.Columns.Add("EmailSubject", typeof(string), null);

                                //To set the Preference values to Database based on OPT STATUS(OPTIN=OPTED_IN(1)/OPTOUT=OPTED_OUT(2))
                                /*--------------------------------------------Start DataProtection-------------------------------*/
                                if (isOptInBehavior == true)
                                {
                                    if (chkRecvPartnersOffrnInfo.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.NONTESCOPROPMATION_BYMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref6.Value) && !string.IsNullOrEmpty(hdnSendMailForDP6.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP6.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.NONTESCOPROPMATION_BYMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                }

                                else
                                {
                                    if (chkRecvPartnersOffrnInfo.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.NONTESCOPROPMATION_BYMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref6.Value) && !string.IsNullOrEmpty(hdnSendMailForDP6.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP6.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.NONTESCOPROPMATION_BYMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                }
                                if (isOptInBehavior == true)
                                {
                                    if (chkRecvTescoOffrnInfo.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCOPROMOTION_BYMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref7.Value) && !string.IsNullOrEmpty(hdnSendMailForDP7.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP7.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCOPROMOTION_BYMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                }
                                else
                                {
                                    if (chkRecvTescoOffrnInfo.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCOPROMOTION_BYMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref7.Value) && !string.IsNullOrEmpty(hdnSendMailForDP7.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP7.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCOPROMOTION_BYMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                }
                                //LCM changes
                                if (isOptInBehavior == true)
                                {
                                    if (chkBonusCoupon.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BONUSCOUPON_BYMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPrefBonus.Value) && !string.IsNullOrEmpty(hdnSendMailForDPBonus.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDPBonus.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BONUSCOUPON_BYMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                }
                                else
                                {
                                    if (chkBonusCoupon.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BONUSCOUPON_BYMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPrefBonus.Value) && !string.IsNullOrEmpty(hdnSendMailForDPBonus.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDPBonus.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BONUSCOUPON_BYMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }

                                }
                                //LCM changes
                                if (isOptInBehavior == true)
                                {
                                    if (chkDontContact.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_BYPHONE.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref8.Value) && !string.IsNullOrEmpty(hdnSendMailForDP8.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP8.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_BYPHONE.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                }
                                else
                                {
                                    if (chkDontContact.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_BYPHONE.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref8.Value) && !string.IsNullOrEmpty(hdnSendMailForDP8.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP8.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_BYPHONE.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                }
                                /*--------------------------------------------End DataProtection-------------------------------*/

                                /*--------------------------------------------Start Contact Preference-------------------------------*/
                                if (radioEmail.Checked)
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.EMAIL_CONTACT.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    if (!Convert.ToBoolean(hdnPref43.Value) && !string.IsNullOrEmpty(hdnSendEmailForEmail.Value))
                                    {
                                        newPreferencerow["EmailSubject"] = hdnSendEmailForEmail.Value;
                                    }
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                else
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.EMAIL_CONTACT.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                if (radioSMS.Checked)
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.MOBILE_CONTACT.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    if (!Convert.ToBoolean(hdnPref44.Value) && !string.IsNullOrEmpty(hdnSendEmailForSMS.Value))
                                    {
                                        newPreferencerow["EmailSubject"] = hdnSendEmailForSMS.Value;
                                    }
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                else
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.MOBILE_CONTACT.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                if (radioPost.Checked)
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.POST_CONTACT.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    if (!Convert.ToBoolean(hdnPref45.Value) && !string.IsNullOrEmpty(hdnSendEmailForPost.Value))
                                    {
                                        newPreferencerow["EmailSubject"] = hdnSendEmailForPost.Value;
                                    }
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                else
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.POST_CONTACT.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                if (chkbraille.Checked)
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.BRAILLE_CONTACT.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                else
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.BRAILLE_CONTACT.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                if (chkLP.Checked)
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.LARGEPRINT_CONTACT.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                else
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.LARGEPRINT_CONTACT.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                /*--------------------------------------------End Contact Preference-------------------------------*/

                                /*--------------------------------------------Start My Clubcard statement Preference-------------------------------*/
                                if (chkXmasSaver.Checked)
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.XMASSAVER.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    if (!Convert.ToBoolean(hdnPref13.Value) && !string.IsNullOrEmpty(hdnSendEmailForChristmasSaver.Value))
                                    {
                                        newPreferencerow["EmailSubject"] = hdnSendEmailForChristmasSaver.Value;
                                    }
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                else
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.XMASSAVER.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                //CCMCA-4700
                                if (chkAviosStandard.Checked)
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.AIRMILES_STD.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    if (!Convert.ToBoolean(hdnPref12.Value) && !string.IsNullOrEmpty(hdnSendEmailForAvios.Value))
                                    {
                                        newPreferencerow["EmailSubject"] = hdnSendEmailForAvios.Value;
                                    }
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                else
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.AIRMILES_STD.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                if (chkAviosPremium.Checked)
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.AIRMILES_PREMIUM.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    if (!Convert.ToBoolean(hdnPref12.Value) && !string.IsNullOrEmpty(hdnSendEmailForAvios.Value))
                                    {
                                        newPreferencerow["EmailSubject"] = hdnSendEmailForAvios.Value;
                                    }
                                    dtPreference.Rows.Add(newPreferencerow);

                                }
                                else
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.AIRMILES_PREMIUM.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                //End
                                if (chkBAviosStandard.Checked)
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.BAMILES_STD.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    if (!Convert.ToBoolean(hdnPref10.Value) && !string.IsNullOrEmpty(hdnSendEmailForBAAvios.Value))
                                    {
                                        newPreferencerow["EmailSubject"] = hdnSendEmailForBAAvios.Value;
                                    }
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                else
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.BAMILES_STD.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                if (cust == 0 && Convert.ToBoolean(hdnIsBnT.Value))
                                {
                                    if (chkBabyTodlerOptIn.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BABYTODLER_CLUB.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref48.Value) && !string.IsNullOrEmpty(hdnSendEmailForBandT.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendEmailForBandT.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else if (chkBabyTodlerOptOut.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BABYTODLER_CLUB.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                }
                                else if (cust == 1 && Convert.ToBoolean(hdnIsBnT.Value))
                                {
                                    if (chkAssoBabyTodlerOptIn.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BABYTODLER_CLUB.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref48.Value) && !string.IsNullOrEmpty(hdnSendEmailForBandT.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendEmailForBandT.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else if (chkAssoBabyTodlerOptOut.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BABYTODLER_CLUB.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                }
                                if (chkBAviosPremium.Checked)
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.BAMILES_PREMIUM.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    if (!Convert.ToBoolean(hdnPref10.Value) && !string.IsNullOrEmpty(hdnSendEmailForBAAvios.Value))
                                    {
                                        newPreferencerow["EmailSubject"] = hdnSendEmailForBAAvios.Value;
                                    }
                                    dtPreference.Rows.Add(newPreferencerow);

                                }
                                else
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.BAMILES_PREMIUM.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                if (chkVirginAtlantic.Checked)
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.VIRGIN.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    if (!Convert.ToBoolean(hdnPref17.Value) && !string.IsNullOrEmpty(hdnSendEmailForVirgin.Value))
                                    {
                                        newPreferencerow["EmailSubject"] = hdnSendEmailForVirgin.Value;
                                    }
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                else
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.VIRGIN.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                if (chkEcoupon.Checked)
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.ECOUPON.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    if (!Convert.ToBoolean(hdnPref15.Value) && !string.IsNullOrEmpty(hdnSendEmailForEcoupon.Value))
                                    {
                                        newPreferencerow["EmailSubject"] = hdnSendEmailForEcoupon.Value;
                                    }
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                else
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.ECOUPON.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }

                                /*--------------------------------------------End My Clubcard statement Preference-------------------------------*/

                                /*--------------------------------------------Start What else can I sign up for? Preference-------------------------------*/
                                //Opt-in of Save Trees.
                                if (chkSaveTree.Checked)
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.SAVETREES.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    if (!Convert.ToBoolean(hdnPref16.Value) && !string.IsNullOrEmpty(hdnSendEmailForSaveTrees.Value))
                                    {
                                        newPreferencerow["EmailSubject"] = hdnSendEmailForSaveTrees.Value;
                                    }
                                    dtPreference.Rows.Add(newPreferencerow);
                                }
                                //Opt-out of Save Trees.
                                else
                                {
                                    DataRow newPreferencerow = dtPreference.NewRow();
                                    newPreferencerow["PreferenceID"] = BusinessConstants.SAVETREES.ToString();
                                    newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                    newPreferencerow["updateDateTime"] = DateTime.Now;
                                    dtPreference.Rows.Add(newPreferencerow);
                                }

                                /*--------------------------------------------End What else can I sign up for? Preference-------------------------------*/

                                /*--------------------------------------------Start Group configurable Preference-------------------------------*/
                                if (DataProtectionTable.Visible)
                                {
                                    if (chkTGMail.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_GROUP_MAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref27.Value) && !string.IsNullOrEmpty(hdnSendMailForDP27.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP27.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_GROUP_MAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    if (chkTGEMail.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_GROUP_EMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref28.Value) && !string.IsNullOrEmpty(hdnSendMailForDP28.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP28.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_GROUP_EMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    if (chkTGPhone.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_GROUP_PHONE.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref29.Value) && !string.IsNullOrEmpty(hdnSendMailForDP29.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP29.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_GROUP_PHONE.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    if (chkTGPhone.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_GROUP_SMS.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref30.Value) && !string.IsNullOrEmpty(hdnSendMailForDP30.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP30.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_GROUP_SMS.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    if (chkTPMail.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_MAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref31.Value) && !string.IsNullOrEmpty(hdnSendMailForDP31.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP31.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_MAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    if (chkTPEmail.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_EMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref32.Value) && !string.IsNullOrEmpty(hdnSendMailForDP32.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP32.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_EMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    if (chkTPPhone.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_PHONE.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref33.Value) && !string.IsNullOrEmpty(hdnSendMailForDP33.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP33.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_PHONE.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    if (chkTPSms.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_SMS.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref34.Value) && !string.IsNullOrEmpty(hdnSendMailForDP34.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP34.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_SMS.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    if (chkRMail.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_MAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref35.Value) && !string.IsNullOrEmpty(hdnSendMailForDP35.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP35.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_MAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    if (chkREmail.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_EMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref36.Value) && !string.IsNullOrEmpty(hdnSendMailForDP36.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP36.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_EMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    if (chkRphone.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_PHONE.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref37.Value) && !string.IsNullOrEmpty(hdnSendMailForDP37.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP37.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_PHONE.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    if (chkRSms.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_SMS.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref38.Value) && !string.IsNullOrEmpty(hdnSendMailForDP38.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP38.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_SMS.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    //LCM Changes
                                    if (chkBCMMail.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BONUSCOUPON_MAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref39.Value) && !string.IsNullOrEmpty(hdnSendMailForDP39.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP39.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BONUSCOUPON_MAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    if (chkBCMEmail.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BONUSCOUPON_EMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref40.Value) && !string.IsNullOrEmpty(hdnSendMailForDP40.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP40.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BONUSCOUPON_EMAIL.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    if (chkBCMPhone.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BONUSCOUPON_PHONE.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref41.Value) && !string.IsNullOrEmpty(hdnSendMailForDP41.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP41.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BONUSCOUPON_PHONE.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    if (chkBCMSms.Checked)
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BONUSCOUPON_SMS.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        if (!Convert.ToBoolean(hdnPref42.Value) && !string.IsNullOrEmpty(hdnSendMailForDP42.Value))
                                        {
                                            newPreferencerow["EmailSubject"] = hdnSendMailForDP42.Value;
                                        }
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    else
                                    {
                                        DataRow newPreferencerow = dtPreference.NewRow();
                                        newPreferencerow["PreferenceID"] = BusinessConstants.BONUSCOUPON_SMS.ToString();
                                        newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                        newPreferencerow["updateDateTime"] = DateTime.Now;
                                        dtPreference.Rows.Add(newPreferencerow);
                                    }
                                    //LCM Changes
                                }
                                //Poland legal changes--Start.
                                if (liGrpTescoproducts.Visible)
                                {
                                    if (isOptInBehavior)
                                    {
                                        if (chkGrpTescoProducts.Checked)
                                        {
                                            DataRow newPreferencerow = dtPreference.NewRow();
                                            newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_GROUP_MAIL.ToString();
                                            newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref27.Value) && !string.IsNullOrEmpty(hdnSendMailForDP27.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP27.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow);

                                            DataRow newPreferencerow1 = dtPreference.NewRow();
                                            newPreferencerow1["PreferenceID"] = BusinessConstants.TESCO_GROUP_EMAIL.ToString();
                                            newPreferencerow1["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow1["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref28.Value) && !string.IsNullOrEmpty(hdnSendMailForDP28.Value))
                                            {
                                                newPreferencerow1["EmailSubject"] = hdnSendMailForDP28.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow1);

                                            DataRow newPreferencerow2 = dtPreference.NewRow();
                                            newPreferencerow2["PreferenceID"] = BusinessConstants.TESCO_GROUP_PHONE.ToString();
                                            newPreferencerow2["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow2["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref29.Value) && !string.IsNullOrEmpty(hdnSendMailForDP29.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP29.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow2);

                                            DataRow newPreferencerow3 = dtPreference.NewRow();
                                            newPreferencerow3["PreferenceID"] = BusinessConstants.TESCO_GROUP_SMS.ToString();
                                            newPreferencerow3["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow3["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref30.Value) && !string.IsNullOrEmpty(hdnSendMailForDP30.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP30.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow3);


                                        }
                                        else
                                        {
                                            DataRow newPreferencerow = dtPreference.NewRow();
                                            newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_GROUP_MAIL.ToString();
                                            newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow);

                                            DataRow newPreferencerow1 = dtPreference.NewRow();
                                            newPreferencerow1["PreferenceID"] = BusinessConstants.TESCO_GROUP_EMAIL.ToString();
                                            newPreferencerow1["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow1["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow1);

                                            DataRow newPreferencerow2 = dtPreference.NewRow();
                                            newPreferencerow2["PreferenceID"] = BusinessConstants.TESCO_GROUP_PHONE.ToString();
                                            newPreferencerow2["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow2["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow2);

                                            DataRow newPreferencerow3 = dtPreference.NewRow();
                                            newPreferencerow3["PreferenceID"] = BusinessConstants.TESCO_GROUP_SMS.ToString();
                                            newPreferencerow3["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow3["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow3);

                                        }
                                    }
                                    else
                                    {
                                        if (chkGrpTescoProducts.Checked)
                                        {
                                            DataRow newPreferencerow = dtPreference.NewRow();
                                            newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_GROUP_MAIL.ToString();
                                            newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow);

                                            DataRow newPreferencerow1 = dtPreference.NewRow();
                                            newPreferencerow1["PreferenceID"] = BusinessConstants.TESCO_GROUP_EMAIL.ToString();
                                            newPreferencerow1["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow1["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow1);

                                            DataRow newPreferencerow2 = dtPreference.NewRow();
                                            newPreferencerow2["PreferenceID"] = BusinessConstants.TESCO_GROUP_PHONE.ToString();
                                            newPreferencerow2["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow2["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow2);

                                            DataRow newPreferencerow3 = dtPreference.NewRow();
                                            newPreferencerow3["PreferenceID"] = BusinessConstants.TESCO_GROUP_SMS.ToString();
                                            newPreferencerow3["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow3["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow3);
                                        }
                                        else
                                        {
                                            DataRow newPreferencerow = dtPreference.NewRow();
                                            newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_GROUP_MAIL.ToString();
                                            newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref27.Value) && !string.IsNullOrEmpty(hdnSendMailForDP27.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP27.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow);

                                            DataRow newPreferencerow1 = dtPreference.NewRow();
                                            newPreferencerow1["PreferenceID"] = BusinessConstants.TESCO_GROUP_EMAIL.ToString();
                                            newPreferencerow1["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow1["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref28.Value) && !string.IsNullOrEmpty(hdnSendMailForDP28.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP28.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow1);

                                            DataRow newPreferencerow2 = dtPreference.NewRow();
                                            newPreferencerow2["PreferenceID"] = BusinessConstants.TESCO_GROUP_PHONE.ToString();
                                            newPreferencerow2["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow2["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref29.Value) && !string.IsNullOrEmpty(hdnSendMailForDP29.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP29.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow2);

                                            DataRow newPreferencerow3 = dtPreference.NewRow();
                                            newPreferencerow3["PreferenceID"] = BusinessConstants.TESCO_GROUP_SMS.ToString();
                                            newPreferencerow3["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow3["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref30.Value) && !string.IsNullOrEmpty(hdnSendMailForDP30.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP30.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow3);
                                        }
                                    }

                                }

                                if (liGrpTescoOffer.Visible)
                                {
                                    if (isOptInBehavior)
                                    {
                                        if (chkGrpPartnerOffers.Checked)
                                        {
                                            DataRow newPreferencerow = dtPreference.NewRow();
                                            newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_MAIL.ToString();
                                            newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref31.Value) && !string.IsNullOrEmpty(hdnSendMailForDP31.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP31.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow);

                                            DataRow newPreferencerow1 = dtPreference.NewRow();
                                            newPreferencerow1["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_EMAIL.ToString();
                                            newPreferencerow1["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow1["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref32.Value) && !string.IsNullOrEmpty(hdnSendMailForDP32.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP32.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow1);

                                            DataRow newPreferencerow2 = dtPreference.NewRow();
                                            newPreferencerow2["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_PHONE.ToString();
                                            newPreferencerow2["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow2["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref33.Value) && !string.IsNullOrEmpty(hdnSendMailForDP33.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP33.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow2);

                                            DataRow newPreferencerow3 = dtPreference.NewRow();
                                            newPreferencerow3["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_SMS.ToString();
                                            newPreferencerow3["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow3["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref34.Value) && !string.IsNullOrEmpty(hdnSendMailForDP34.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP34.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow3);


                                        }
                                        else
                                        {
                                            DataRow newPreferencerow = dtPreference.NewRow();
                                            newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_MAIL.ToString();
                                            newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow);

                                            DataRow newPreferencerow1 = dtPreference.NewRow();
                                            newPreferencerow1["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_EMAIL.ToString();
                                            newPreferencerow1["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow1["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow1);

                                            DataRow newPreferencerow2 = dtPreference.NewRow();
                                            newPreferencerow2["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_PHONE.ToString();
                                            newPreferencerow2["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow2["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow2);

                                            DataRow newPreferencerow3 = dtPreference.NewRow();
                                            newPreferencerow3["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_SMS.ToString();
                                            newPreferencerow3["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow3["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow3);

                                        }
                                    }
                                    else
                                    {
                                        if (chkGrpPartnerOffers.Checked)
                                        {
                                            DataRow newPreferencerow = dtPreference.NewRow();
                                            newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_MAIL.ToString();
                                            newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow);

                                            DataRow newPreferencerow1 = dtPreference.NewRow();
                                            newPreferencerow1["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_EMAIL.ToString();
                                            newPreferencerow1["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow1["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow1);

                                            DataRow newPreferencerow2 = dtPreference.NewRow();
                                            newPreferencerow2["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_PHONE.ToString();
                                            newPreferencerow2["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow2["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow2);

                                            DataRow newPreferencerow3 = dtPreference.NewRow();
                                            newPreferencerow3["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_SMS.ToString();
                                            newPreferencerow3["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow3["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow3);
                                        }
                                        else
                                        {
                                            DataRow newPreferencerow = dtPreference.NewRow();
                                            newPreferencerow["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_MAIL.ToString();
                                            newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref31.Value) && !string.IsNullOrEmpty(hdnSendMailForDP31.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP31.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow);

                                            DataRow newPreferencerow1 = dtPreference.NewRow();
                                            newPreferencerow1["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_EMAIL.ToString();
                                            newPreferencerow1["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow1["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref32.Value) && !string.IsNullOrEmpty(hdnSendMailForDP32.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP32.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow1);

                                            DataRow newPreferencerow2 = dtPreference.NewRow();
                                            newPreferencerow2["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_PHONE.ToString();
                                            newPreferencerow2["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow2["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref33.Value) && !string.IsNullOrEmpty(hdnSendMailForDP33.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP33.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow2);

                                            DataRow newPreferencerow3 = dtPreference.NewRow();
                                            newPreferencerow3["PreferenceID"] = BusinessConstants.TESCO_THIRD_PARTY_SMS.ToString();
                                            newPreferencerow3["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow3["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref34.Value) && !string.IsNullOrEmpty(hdnSendMailForDP34.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP34.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow3);
                                        }
                                    }

                                }

                                if (liGrpTescoCustomerReasearch.Visible)
                                {
                                    if (isOptInBehavior)
                                    {
                                        if (chkGrpResearch.Checked)
                                        {
                                            DataRow newPreferencerow = dtPreference.NewRow();
                                            newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_MAIL.ToString();
                                            newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref35.Value) && !string.IsNullOrEmpty(hdnSendMailForDP35.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP35.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow);

                                            DataRow newPreferencerow1 = dtPreference.NewRow();
                                            newPreferencerow1["PreferenceID"] = BusinessConstants.RESEARCH_EMAIL.ToString();
                                            newPreferencerow1["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow1["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref36.Value) && !string.IsNullOrEmpty(hdnSendMailForDP36.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP36.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow1);

                                            DataRow newPreferencerow2 = dtPreference.NewRow();
                                            newPreferencerow2["PreferenceID"] = BusinessConstants.RESEARCH_PHONE.ToString();
                                            newPreferencerow2["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow2["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref37.Value) && !string.IsNullOrEmpty(hdnSendMailForDP37.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP37.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow2);

                                            DataRow newPreferencerow3 = dtPreference.NewRow();
                                            newPreferencerow3["PreferenceID"] = BusinessConstants.RESEARCH_SMS.ToString();
                                            newPreferencerow3["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow3["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref38.Value) && !string.IsNullOrEmpty(hdnSendMailForDP38.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP38.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow3);


                                        }
                                        else
                                        {
                                            DataRow newPreferencerow = dtPreference.NewRow();
                                            newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_MAIL.ToString();
                                            newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow);

                                            DataRow newPreferencerow1 = dtPreference.NewRow();
                                            newPreferencerow1["PreferenceID"] = BusinessConstants.RESEARCH_EMAIL.ToString();
                                            newPreferencerow1["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow1["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow1);

                                            DataRow newPreferencerow2 = dtPreference.NewRow();
                                            newPreferencerow2["PreferenceID"] = BusinessConstants.RESEARCH_PHONE.ToString();
                                            newPreferencerow2["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow2["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow2);

                                            DataRow newPreferencerow3 = dtPreference.NewRow();
                                            newPreferencerow3["PreferenceID"] = BusinessConstants.RESEARCH_SMS.ToString();
                                            newPreferencerow3["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow3["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow3);

                                        }
                                    }
                                    else
                                    {
                                        if (chkGrpResearch.Checked)
                                        {
                                            DataRow newPreferencerow = dtPreference.NewRow();
                                            newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_MAIL.ToString();
                                            newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow);

                                            DataRow newPreferencerow1 = dtPreference.NewRow();
                                            newPreferencerow1["PreferenceID"] = BusinessConstants.RESEARCH_EMAIL.ToString();
                                            newPreferencerow1["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow1["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow1);

                                            DataRow newPreferencerow2 = dtPreference.NewRow();
                                            newPreferencerow2["PreferenceID"] = BusinessConstants.RESEARCH_PHONE.ToString();
                                            newPreferencerow2["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow2["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow2);

                                            DataRow newPreferencerow3 = dtPreference.NewRow();
                                            newPreferencerow3["PreferenceID"] = BusinessConstants.RESEARCH_SMS.ToString();
                                            newPreferencerow3["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow3["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow3);
                                        }
                                        else
                                        {
                                            DataRow newPreferencerow = dtPreference.NewRow();
                                            newPreferencerow["PreferenceID"] = BusinessConstants.RESEARCH_MAIL.ToString();
                                            newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref35.Value) && !string.IsNullOrEmpty(hdnSendMailForDP35.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP35.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow);

                                            DataRow newPreferencerow1 = dtPreference.NewRow();
                                            newPreferencerow1["PreferenceID"] = BusinessConstants.RESEARCH_EMAIL.ToString();
                                            newPreferencerow1["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow1["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref36.Value) && !string.IsNullOrEmpty(hdnSendMailForDP36.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP36.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow1);

                                            DataRow newPreferencerow2 = dtPreference.NewRow();
                                            newPreferencerow2["PreferenceID"] = BusinessConstants.RESEARCH_PHONE.ToString();
                                            newPreferencerow2["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow2["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref37.Value) && !string.IsNullOrEmpty(hdnSendMailForDP37.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP37.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow2);

                                            DataRow newPreferencerow3 = dtPreference.NewRow();
                                            newPreferencerow3["PreferenceID"] = BusinessConstants.RESEARCH_SMS.ToString();
                                            newPreferencerow3["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow3["updateDateTime"] = DateTime.Now;
                                            if (!Convert.ToBoolean(hdnPref38.Value) && !string.IsNullOrEmpty(hdnSendMailForDP38.Value))
                                            {
                                                newPreferencerow["EmailSubject"] = hdnSendMailForDP38.Value;
                                            }
                                            dtPreference.Rows.Add(newPreferencerow3);
                                        }
                                    }

                                }
                                //Poland legal changes--End.


                                //-------------------------------------------Start Baby Todler--------------------------------------------------------
                                ClubDetails objClubDetails = new ClubDetails();
                                if (liBabyTodlerOptIn.Visible || liBabyTodlerOptOut.Visible || chkAviosPremium.Checked || chkAviosStandard.Checked || chkBAviosStandard.Checked || chkBAviosPremium.Checked || chkVirginAtlantic.Checked)
                                {
                                    DataTable dtClubDeatils = new DataTable();
                                    DataTable dtDOB = new DataTable();
                                    DataTable dtBTDetails = new DataTable();
                                    dtBTDetails.Columns.Add("DateOfBirth", typeof(string), null);
                                    dtBTDetails.Columns.Add("OriginalDateOfBirth", typeof(string), null);

                                    List<ClubDetails> dateList = new List<ClubDetails>();
                                    List<ClubDetails> mediaList = new List<ClubDetails>();

                                    //CCMCA-4700
                                    if (isAviosMembershipEnable)
                                    {
                                        if (chkAviosStandard.Checked)
                                        {
                                            ClubDetails objClubs = new ClubDetails
                                            {
                                                MembershipID = txtAviosMembership.Text.Trim(),
                                                IsDeleted = "N",
                                                ClubID = BusinessConstants.CLUB_AVIOS
                                            };
                                            mediaList.Add(objClubs);
                                        }
                                        else if (chkAviosPremium.Checked)
                                        {
                                            ClubDetails objClubs = new ClubDetails
                                            {
                                                MembershipID = txtAviosMembership.Text.Trim(),
                                                IsDeleted = "N",
                                                ClubID = BusinessConstants.CLUB_AVIOS
                                            };
                                            mediaList.Add(objClubs);
                                        }
                                        else
                                        {
                                            ClubDetails objClubs = new ClubDetails
                                            {
                                                MembershipID = string.Empty,
                                                IsDeleted = "Y",
                                                ClubID = BusinessConstants.CLUB_AVIOS
                                            };
                                            mediaList.Add(objClubs);
                                        }
                                    }

                                    ////END

                                    if (chkBAviosStandard.Checked)
                                    {
                                        ClubDetails objClubs = new ClubDetails
                                        {
                                            MembershipID = txtBAvios.Text.Trim(),
                                            IsDeleted = "N",
                                            ClubID = BusinessConstants.CLUB_BA
                                        };
                                        mediaList.Add(objClubs);
                                    }
                                    else if (chkBAviosPremium.Checked)
                                    {
                                        ClubDetails objClubs = new ClubDetails
                                        {
                                            MembershipID = txtBAvios.Text.Trim(),
                                            IsDeleted = "N",
                                            ClubID = BusinessConstants.CLUB_BA
                                        };
                                        mediaList.Add(objClubs);
                                    }
                                    else
                                    {
                                        ClubDetails objClubs = new ClubDetails
                                        {
                                            MembershipID = string.Empty,
                                            IsDeleted = "Y",
                                            ClubID = BusinessConstants.CLUB_BA
                                        };
                                        mediaList.Add(objClubs);
                                    }

                                    if (chkVirginAtlantic.Checked)
                                    {
                                        ClubDetails objClubs = new ClubDetails
                                        {
                                            MembershipID = txtVirgnMembershipID.Text.Trim(),
                                            IsDeleted = "N",
                                            ClubID = BusinessConstants.CLUB_VIRGIN
                                        };
                                        mediaList.Add(objClubs);
                                    }
                                    else
                                    {
                                        ClubDetails objClubs = new ClubDetails
                                        {
                                            MembershipID = string.Empty,
                                            IsDeleted = "Y",
                                            ClubID = BusinessConstants.CLUB_VIRGIN
                                        };
                                        mediaList.Add(objClubs);
                                    }
                                    if (cust == 0)
                                    {
                                        dtClubDeatils = (DataTable)ViewState["BTOriginalDetails"];
                                        dtDOB = (DataTable)ViewState["BTDetails"];
                                        if (chkBabyTodlerOptIn.Checked && liBabyTodlerOptIn.Visible)
                                        {
                                            ClubDetails objClubs = new ClubDetails
                                            {
                                                MembershipID = "",
                                                IsDeleted = "N",
                                                ClubID = BusinessConstants.CLUB_BT
                                            };
                                            mediaList.Add(objClubs);

                                            if (dtClubDeatils.Rows.Count > grdBabyTodlerOptIn.Rows.Count)
                                            {
                                                for (int i = 0; i < dtClubDeatils.Rows.Count; i++)
                                                {
                                                    if (i >= grdBabyTodlerOptIn.Rows.Count)
                                                    {
                                                        dtBTDetails.Rows.Add(null, dtClubDeatils.Rows[i]["OriginalDateOfBirth"].ToString());
                                                    }
                                                    else
                                                    {
                                                        TextBox txtDOB = (TextBox)grdBabyTodlerOptIn.Rows[i].Cells[1].FindControl("txtDOB");
                                                        dtBTDetails.Rows.Add(txtDOB.Text.Trim(), dtClubDeatils.Rows[i]["OriginalDateOfBirth"].ToString());
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (int i = 0; i < grdBabyTodlerOptIn.Rows.Count; i++)
                                                {
                                                    TextBox txtDOB = (TextBox)grdBabyTodlerOptIn.Rows[i].Cells[1].FindControl("txtDOB");
                                                    if (i >= dtClubDeatils.Rows.Count)
                                                    {
                                                        dtBTDetails.Rows.Add(txtDOB.Text, null);
                                                    }
                                                    else
                                                    {
                                                        dtBTDetails.Rows.Add(txtDOB.Text, dtClubDeatils.Rows[i]["OriginalDateOfBirth"].ToString());
                                                    }
                                                }
                                            }
                                            dtBTDetails.AcceptChanges();
                                            //
                                        }
                                        else if (liBabyTodlerOptOut.Visible && !(chkBabyTodlerOptOut.Checked))
                                        {
                                            ClubDetails objClubs = new ClubDetails
                                            {
                                                MembershipID = "",
                                                IsDeleted = "N",
                                                ClubID = BusinessConstants.CLUB_BT
                                            };
                                            mediaList.Add(objClubs);

                                            if (dtClubDeatils.Rows.Count > grdBabyTodlerOptOut.Rows.Count)
                                            {
                                                for (int i = 0; i < dtClubDeatils.Rows.Count; i++)
                                                {
                                                    if (i >= grdBabyTodlerOptOut.Rows.Count)
                                                    {
                                                        dtBTDetails.Rows.Add(null, dtClubDeatils.Rows[i]["OriginalDateOfBirth"].ToString());
                                                    }
                                                    else
                                                    {
                                                        TextBox txtDOB = (TextBox)grdBabyTodlerOptOut.Rows[i].Cells[1].FindControl("txtDOB");
                                                        dtBTDetails.Rows.Add(txtDOB.Text.Trim(), dtClubDeatils.Rows[i]["OriginalDateOfBirth"].ToString());
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (int i = 0; i < grdBabyTodlerOptOut.Rows.Count; i++)
                                                {
                                                    TextBox txtDOB = (TextBox)grdBabyTodlerOptOut.Rows[i].Cells[1].FindControl("txtDOB");
                                                    if (i >= dtClubDeatils.Rows.Count)
                                                    {
                                                        dtBTDetails.Rows.Add(txtDOB.Text, null);
                                                    }
                                                    else
                                                    {
                                                        dtBTDetails.Rows.Add(txtDOB.Text, dtClubDeatils.Rows[i]["OriginalDateOfBirth"].ToString());
                                                    }
                                                }
                                            }
                                            dtBTDetails.AcceptChanges();

                                        }
                                        else if (liBabyTodlerOptOut.Visible && chkBabyTodlerOptOut.Checked)
                                        {
                                            chkBabyTodlerOptOut.Checked = false;
                                            ClubDetails objClubs = new ClubDetails
                                            {
                                                MembershipID = "",
                                                IsDeleted = "Y",
                                                ClubID = BusinessConstants.CLUB_BT
                                            };
                                            mediaList.Add(objClubs);
                                            dtBTDetails.Rows.Clear();
                                        }
                                        objClubDetails.ClubInformation = mediaList;

                                        if (dtBTDetails.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtBTDetails.Rows.Count; i++)
                                            {
                                                ClubDetails objClubs = new ClubDetails
                                                {
                                                    DateOfBirth = dtBTDetails.Rows[i]["OriginalDateOfBirth"].ToString().Trim(),
                                                    ChangedBirthDate = dtBTDetails.Rows[i]["DateOfBirth"].ToString().Trim()
                                                };
                                                dateList.Add(objClubs);
                                            }
                                        }
                                    }
                                    ///////////// Associate Customer
                                    else if (cust == 1)
                                    {

                                        dtClubDeatils = (DataTable)ViewState["BTAssoOriginalDetails"];
                                        dtDOB = (DataTable)ViewState["BTAssoDetails"];
                                        if (chkAssoBabyTodlerOptIn.Checked && liAssoBabyTodlerOptIn.Visible)
                                        {
                                            ClubDetails objClubs = new ClubDetails
                                            {
                                                MembershipID = "",
                                                IsDeleted = "N",
                                                ClubID = BusinessConstants.CLUB_BT
                                            };
                                            mediaList.Add(objClubs);

                                            if (dtClubDeatils.Rows.Count > grdBabyTodlerAssoOptIn.Rows.Count)
                                            {
                                                for (int i = 0; i < dtClubDeatils.Rows.Count; i++)
                                                {
                                                    if (i >= grdBabyTodlerAssoOptIn.Rows.Count)
                                                    {
                                                        dtBTDetails.Rows.Add(null, dtClubDeatils.Rows[i]["OriginalDateOfBirth"].ToString());
                                                    }
                                                    else
                                                    {
                                                        TextBox txtDOB = (TextBox)grdBabyTodlerAssoOptIn.Rows[i].Cells[1].FindControl("txtDOB");
                                                        dtBTDetails.Rows.Add(txtDOB.Text.Trim(), dtClubDeatils.Rows[i]["OriginalDateOfBirth"].ToString());
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (int i = 0; i < grdBabyTodlerAssoOptIn.Rows.Count; i++)
                                                {
                                                    TextBox txtDOB = (TextBox)grdBabyTodlerAssoOptIn.Rows[i].Cells[1].FindControl("txtDOB");
                                                    if (i >= dtClubDeatils.Rows.Count)
                                                    {
                                                        dtBTDetails.Rows.Add(txtDOB.Text, null);
                                                    }
                                                    else
                                                    {
                                                        dtBTDetails.Rows.Add(txtDOB.Text, dtClubDeatils.Rows[i]["OriginalDateOfBirth"].ToString());
                                                    }
                                                }
                                            }
                                            dtBTDetails.AcceptChanges();
                                            //
                                        }
                                        else if (liAssoBabyTodlerOptOut.Visible && !(chkAssoBabyTodlerOptOut.Checked))
                                        {
                                            ClubDetails objClubs = new ClubDetails
                                            {
                                                MembershipID = "",
                                                IsDeleted = "N",
                                                ClubID = BusinessConstants.CLUB_BT
                                            };
                                            mediaList.Add(objClubs);

                                            if (dtClubDeatils.Rows.Count > grdBabyTodlerAssoOptOut.Rows.Count)
                                            {
                                                for (int i = 0; i < dtClubDeatils.Rows.Count; i++)
                                                {
                                                    if (i >= grdBabyTodlerAssoOptOut.Rows.Count)
                                                    {
                                                        dtBTDetails.Rows.Add(null, dtClubDeatils.Rows[i]["OriginalDateOfBirth"].ToString());
                                                    }
                                                    else
                                                    {
                                                        TextBox txtDOB = (TextBox)grdBabyTodlerAssoOptOut.Rows[i].Cells[1].FindControl("txtDOB");
                                                        dtBTDetails.Rows.Add(txtDOB.Text.Trim(), dtClubDeatils.Rows[i]["OriginalDateOfBirth"].ToString());
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (int i = 0; i < grdBabyTodlerAssoOptOut.Rows.Count; i++)
                                                {
                                                    TextBox txtDOB = (TextBox)grdBabyTodlerAssoOptOut.Rows[i].Cells[1].FindControl("txtDOB");
                                                    if (i >= dtClubDeatils.Rows.Count)
                                                    {
                                                        dtBTDetails.Rows.Add(txtDOB.Text, null);
                                                    }
                                                    else
                                                    {
                                                        dtBTDetails.Rows.Add(txtDOB.Text, dtClubDeatils.Rows[i]["OriginalDateOfBirth"].ToString());
                                                    }
                                                }
                                            }
                                            dtBTDetails.AcceptChanges();

                                        }
                                        else if (liAssoBabyTodlerOptOut.Visible && chkAssoBabyTodlerOptOut.Checked)
                                        {
                                            chkAssoBabyTodlerOptOut.Checked = false;
                                            ClubDetails objClubs = new ClubDetails
                                            {
                                                MembershipID = "",
                                                IsDeleted = "Y",
                                                ClubID = BusinessConstants.CLUB_BT
                                            };
                                            mediaList.Add(objClubs);
                                            dtBTDetails.Rows.Clear();
                                        }
                                        objClubDetails.ClubInformation = mediaList;

                                        if (dtBTDetails.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtBTDetails.Rows.Count; i++)
                                            {
                                                ClubDetails objClubs = new ClubDetails
                                                {
                                                    DateOfBirth = dtBTDetails.Rows[i]["OriginalDateOfBirth"].ToString().Trim(),
                                                    ChangedBirthDate = dtBTDetails.Rows[i]["DateOfBirth"].ToString().Trim()
                                                };
                                                dateList.Add(objClubs);
                                            }
                                        }
                                    }

                                    objClubDetails.DOBDetails = dateList;
                                    objClubDetails.JoinDate = DateTime.Now;

                                }

                                //-------------------------------------------End Baby Todler-------------------------------------------------------

                                /*--------------------------------------------End Group configurable Preference-------------------------------*/

                                //-----------------------------------------------------Start Customer Update Details-------------------------------

                                Hashtable htCustomer = new Hashtable();
                                if (!string.IsNullOrEmpty(txtBTMobile.Text.Trim()) && radioSMS.Checked)
                                {
                                    htCustomer["mobile_phone_number"] = this.txtBTMobile.Text.Trim();
                                    htCustomer["MobilePhoneNumber"] = this.txtBTMobile.Text.Trim();
                                    htCustomer["CustomerMobilePhoneStatus"] = BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE;

                                }
                                else if (hdnSms.Value != "")
                                {
                                    htCustomer["mobile_phone_number"] = hdnSms.Value;
                                    htCustomer["MobilePhoneNumber"] = hdnSms.Value;
                                    if (!string.IsNullOrEmpty(hdnCustomerMobilePhoneStatus.Value))
                                    {
                                        htCustomer["CustomerMobilePhoneStatus"] = hdnCustomerMobilePhoneStatus.Value;
                                    }
                                    else
                                    {
                                        htCustomer["CustomerMobilePhoneStatus"] = BusinessConstants.CUSTOMERMAILSTATUS_MISSING;
                                    }
                                }
                                if (!string.IsNullOrEmpty(txtBTEmail.Text.Trim()) && radioEmail.Checked)
                                {
                                    htCustomer["EmailAddress"] = this.txtBTEmail.Text.Trim();
                                    htCustomer["email_address"] = this.txtBTEmail.Text.Trim();
                                    htCustomer["CustomerEmailStatus"] = BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE;
                                }
                                else if (hdnEmail.Value != "")
                                {
                                    htCustomer["EmailAddress"] = hdnEmail.Value;
                                    htCustomer["email_address"] = hdnEmail.Value;
                                    if (!string.IsNullOrEmpty(hdnCustomerEmailStatus.Value))
                                    {
                                        htCustomer["CustomerEmailStatus"] = hdnCustomerEmailStatus.Value;
                                    }
                                    else
                                    {
                                        htCustomer["CustomerEmailStatus"] = BusinessConstants.CUSTOMERMAILSTATUS_MISSING;
                                    }
                                }
                                if (!string.IsNullOrEmpty(hdnCustomerUseStatusMain.Value))
                                {
                                    htCustomer["CustomerUseStatusMain"] = hdnCustomerUseStatusMain.Value;
                                }
                                else
                                {
                                    htCustomer["CustomerUseStatusMain"] = BusinessConstants.CUSTOMERUSESTATUS_ACTIVE;
                                }
                                htCustomer["TitleEnglish"] = hdnTiltle.Value;
                                htCustomer["Name1"] = hdnName1.Value;
                                htCustomer["Name2"] = hdnName2.Value;
                                htCustomer["Name3"] = hdnName3.Value;
                                if (!string.IsNullOrEmpty(hdnDOB.Value))
                                {
                                    htCustomer["DateOfBirth"] = hdnDOB.Value;
                                }
                                htCustomer["Sex"] = hdnSex.Value;
                                htCustomer["MailingAddressLine1"] = hdnMailingAddress1.Value;
                                htCustomer["MailingAddressLine2"] = hdnMailingAddress2.Value;
                                htCustomer["MailingAddressLine3"] = hdnMailingAddress3.Value;
                                htCustomer["MailingAddressLine4"] = hdnMailingAddress4.Value;
                                htCustomer["MailingAddressLine5"] = hdnMailingAddress5.Value;
                                htCustomer["MailingAddressLine6"] = hdnMailingAddress6.Value;
                                htCustomer["MailingAddressPostCode"] = hdnMailingAddressPostcode.Value;
                                htCustomer["evening_phone_number"] = hdnEveningPhone.Value;
                                htCustomer["daytime_phone_number"] = hdnDayTimePhone.Value;
                                htCustomer["SSN"] = hdnSSN.Value;
                                htCustomer["PassportNo"] = hdnPassport.Value;
                                if (!string.IsNullOrEmpty(hdnCustomerMailStatus.Value))
                                {
                                    htCustomer["CustomerMailStatus"] = hdnCustomerMailStatus.Value;
                                }
                                else
                                {
                                    htCustomer["CustomerMailStatus"] = BusinessConstants.CUSTOMERMAILSTATUS_MISSING;
                                }
                                if (!string.IsNullOrEmpty(hdnRaceID.Value))
                                {
                                    htCustomer["RaceID"] = hdnRaceID.Value;
                                }
                                else
                                {
                                    htCustomer["RaceID"] = 0;
                                }
                                htCustomer["ISOLanguageCode"] = hdnLanguage.Value;
                                htCustomer["Culture"] = culture;
                                htCustomer["number_of_household_members"] = Convert.ToInt16(hdnTotalHouseHoldMembers.Value);
                                if (!string.IsNullOrEmpty(hdnAge1.Value))
                                {
                                    htCustomer["family_member_1_dob"] = GetDateOfBirth(Convert.ToInt16(hdnAge1.Value)).ToString();
                                }
                                if (!string.IsNullOrEmpty(hdnAge2.Value))
                                {
                                    htCustomer["family_member_2_dob"] = GetDateOfBirth(Convert.ToInt16(hdnAge2.Value)).ToString();
                                }
                                if (!string.IsNullOrEmpty(hdnAge3.Value))
                                {
                                    htCustomer["family_member_3_dob"] = GetDateOfBirth(Convert.ToInt16(hdnAge3.Value)).ToString();
                                }
                                if (!string.IsNullOrEmpty(hdnAge4.Value))
                                {
                                    htCustomer["family_member_4_dob"] = GetDateOfBirth(Convert.ToInt16(hdnAge4.Value)).ToString();
                                }
                                if (!string.IsNullOrEmpty(hdnAge5.Value))
                                {
                                    htCustomer["family_member_5_dob"] = GetDateOfBirth(Convert.ToInt16(hdnAge5.Value)).ToString();
                                }

                                htCustomer["CustomerID"] = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                                string strXml = Helper.HashTableToXML(htCustomer, "customer");

                                //-----------------------------------------------------End Customer Update Details-------------------------------

                                string inputXml = string.Empty;
                                string errorXml = string.Empty;
                                joinServiceClient = new JoinLoyaltyServiceClient();
                                bool breturnflag = true;

                                //Duplicate check
                                if (joinServiceClient.AccountDuplicateCheck(out inputXml, strXml))
                                {
                                    yourchangessaved.Visible = true;
                                    resulDoc = new XmlDocument();
                                    resulDoc.LoadXml(inputXml);
                                    DataSet dsJoin = new DataSet();
                                    dsJoin.ReadXml(new XmlNodeReader(resulDoc));
                                    if (dsJoin.Tables["Duplicate"].Rows[0].ItemArray[0].ToString().Trim() == "1" || dsJoin.Tables["Duplicate"].Rows[0].ItemArray[2].ToString().Trim() == "1")
                                    {
                                        //lclChangesSaved.Text = "It appears that there is another account with this email address/mobile number.";
                                        lclChangesSaved.Text = GetLocalResourceObject("Mobilevali.Text").ToString(); //Mobilevali.Text
                                        btnConfirmPreferences.Enabled = true;
                                        if (hdnBTCheck.Value == "1")
                                        {
                                            chkBabyTodlerOptIn.Checked = true;
                                            //txtBTEmail.Enabled = true;
                                            //txtBTMobile.Enabled = true;
                                            grdBabyTodlerOptIn.Enabled = true;
                                            divBTOptIn.Attributes["class"] = "";
                                            divOptInBT.Attributes.Add("style", "display:block");
                                            divOptInBT.Attributes.Add("style", "width:100%");
                                        }
                                        if (hdnBTAssoCheck.Value == "1")
                                        {
                                            chkAssoBabyTodlerOptIn.Checked = true;
                                            //txtBTEmail.Enabled = true;
                                            //txtBTMobile.Enabled = true;
                                            grdBabyTodlerAssoOptIn.Enabled = true;
                                            divAssoOptInBT.Attributes["class"] = "";
                                            divOptInBTAsso.Attributes.Add("style", "display:block");
                                            divOptInBTAsso.Attributes.Add("style", "width:100%");
                                        }
                                    }
                                    else
                                    {
                                        consumer = Helper.GetTripleDESEncryptedCookieValue("UserName").ToString();
                                        #region Trace Start
                                        NGCTrace.NGCTrace.TraceInfo("Start: CSC Preferences.LoadPreferences()");
                                        NGCTrace.NGCTrace.TraceDebug("Start: CSC Preferences.LoadPreferences() Input Xml-" + consumer);
                                        #endregion
                                        preferenceserviceClient = new PreferenceServiceClient();
                                        //consumer = ConfigurationSettings.AppSettings["ServiceConsumer"];
                                        CustomerPreference objcustPref = new CustomerPreference();
                                        objcustPref.CustomerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
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
                                        objClubDetails.Culture = culture;
                                        objClubDetails.UserID = consumer;
                                        if (cust == 0)
                                        {
                                            DataSet dsMyAccountDetails = new DataSet();
                                            bool boolResult = false;

                                            CustomerDetails objCustomerDetails = new CustomerDetails();
                                            objCustomerDetails.EmailId = txtBTEmail.Text.ToString().Trim();
                                            objCustomerDetails.Surname = hdnName3.Value;
                                            objCustomerDetails.Title = hdnTiltle.Value;
                                            objCustomerDetails.Firstname = hdnName1.Value;

                                            //To get the clubcard number 
                                            clubcardserviceClient = new ClubcardServiceClient();

                                            boolResult = clubcardserviceClient.GetMyAccountDetails(out errorXml, out resultXml, Convert.ToInt64(hdnPrimaryCustID.Value), culture);
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

                                            preferenceserviceClient.MaintainCustomerPreference(Convert.ToInt64(hdnPrimaryCustID.Value), objcustPref, objCustomerDetails);
                                            if (Convert.ToBoolean(hdnIsBnT.Value))
                                            {
                                                if (hdnBTCheck.Value == "1")
                                                {
                                                    preferenceserviceClient.MaintainClubDetails(Convert.ToInt64(hdnPrimaryCustID.Value), objClubDetails, txtBTEmail.Text.Trim());
                                                    chkBabyTodlerOptIn.Checked = false; // set opt in checkbox to uncheck state
                                                    hdnBTCheck.Value = "0";
                                                }
                                                else
                                                {
                                                    chkBabyTodlerOptOut.Checked = false;
                                                    preferenceserviceClient.MaintainClubDetails(Convert.ToInt64(hdnPrimaryCustID.Value), objClubDetails, "");
                                                    divOptInBT.Attributes.Add("style", "display:none");
                                                }
                                                LoadBnTPreferences(Convert.ToInt64(hdnPrimaryCustID.Value), 0);
                                            }
                                        }
                                        else if (cust == 1)
                                        {
                                            DataSet dsMyAccountDetails = new DataSet();
                                            bool boolResult = false;

                                            CustomerDetails objCustomerDetails = new CustomerDetails();
                                            objCustomerDetails.EmailId = txtBTEmail.Text.ToString().Trim();
                                            objCustomerDetails.Surname = hdnName3.Value;
                                            objCustomerDetails.Title = hdnTiltle.Value;
                                            objCustomerDetails.Firstname = hdnName1.Value;

                                            //To get the clubcard number 
                                            clubcardserviceClient = new ClubcardServiceClient();

                                            boolResult = clubcardserviceClient.GetMyAccountDetails(out errorXml, out resultXml, Convert.ToInt64(hdnAssociateCustID.Value), culture);
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


                                            preferenceserviceClient.MaintainCustomerPreference(Convert.ToInt64(hdnAssociateCustID.Value), objcustPref, objCustomerDetails);
                                            if (Convert.ToBoolean(hdnIsBnT.Value))
                                            {
                                                if (hdnBTAssoCheck.Value == "1")
                                                {
                                                    preferenceserviceClient.MaintainClubDetails(Convert.ToInt64(hdnAssociateCustID.Value), objClubDetails, txtBTEmail.Text.Trim());
                                                    chkAssoBabyTodlerOptIn.Checked = false; // set opt in checkbox to uncheck state
                                                    hdnBTAssoCheck.Value = "0";
                                                }
                                                else
                                                {
                                                    chkAssoBabyTodlerOptOut.Checked = false;
                                                    preferenceserviceClient.MaintainClubDetails(Convert.ToInt64(hdnAssociateCustID.Value), objClubDetails, "");
                                                    divOptInBTAsso.Attributes.Add("style", "display:none");
                                                }
                                                LoadBnTPreferences(Convert.ToInt64(hdnAssociateCustID.Value), 1);
                                            }
                                        }

                                        yourchangessaved.Visible = true;
                                        ResetHiddenValues();
                                        LoadPreferences();
                                        LoadMemmberShip();
                                        customerObj = new CustomerServiceClient();
                                        if (customerObj.UpdateCustomerDetails(out errorXml, out customerID, strXml, consumer))
                                        {
                                            amendDateTime = DateTime.UtcNow.ToString(dateFormat);
                                            lclChangesSaved.Text = GetLocalResourceObject("Savetext.Text").ToString();//"Your changes have been saved";//Savetext.Text
                                            btnConfirmPreferences.Enabled = true;
                                            if (!radioEmail.Checked && !txtBTEmail.ReadOnly)
                                                txtBTEmail.Text = string.Empty;
                                            if (!radioSMS.Checked && !txtBTMobile.ReadOnly)
                                                txtBTMobile.Text = string.Empty;

                                            lblCustomerLastUpdatedBy.Text = consumer + " @ " + amendDateTime;
                                            Helper.SetTripleDESEncryptedCookie("lblCustomerLastAmendedBy", consumer);
                                            Helper.SetTripleDESEncryptedCookie("lblCustomerLastAmendDate", amendDateTime);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Preferences.LoadPreferences()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Preferences.LoadPreferences() Input Xml-" + consumer);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.LoadPreferences() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.LoadPreferences() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.LoadPreferences()");
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
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Preferences.SetHouseHoldStatus()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Preferences.SetHouseHoldStatus() Clubcard CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
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

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Preferences.SetHouseHoldStatus()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Preferences.SetHouseHoldStatus() Clubcard CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Preferences.SetHouseHoldStatus() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Preferences.SetHouseHoldStatus() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Preferences.SetHouseHoldStatus()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
            finally
            {

            }
        }
        #endregion

        ///// <summary>
        ///// LoadCustomerDetails to load the email and PhoneNumber fields and other customer details
        ///// </summary>
        //public void LoadCustomerDetails()
        //{
        //    int rowCount, maxRows;
        //    Hashtable searchData = new Hashtable();

        //    resultXml = string.Empty;
        //    string conditionXML = string.Empty;
        //    errorXml = string.Empty;
        //    rowCount = 0;
        //    searchData["CustomerID"] = Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString();
        //    conditionXML = Helper.HashTableToXML(searchData, "customer");
        //    maxRows = 100;
        //    DataSet dsCustomer = null;

        //    CustomerServiceClient serviceClient = new CustomerServiceClient();

        //    if (serviceClient.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, maxRows, culture))
        //    {
        //        resulDoc = new XmlDocument();
        //        resulDoc.LoadXml(resultXml);
        //        dsCustomer = new DataSet();
        //        dsCustomer.ReadXml(new XmlNodeReader(resulDoc));
        //        if (dsCustomer != null)
        //        {
        //            if (dsCustomer.Tables["Customer"].Columns.Contains("email_address"))
        //            {
        //                hdnEmail.Value = dsCustomer.Tables["Customer"].Rows[0]["email_address"].ToString().Trim();
        //            }
        //            if (dsCustomer.Tables["Customer"].Columns.Contains("mobile_phone_number"))
        //            {
        //                hdnSms.Value = dsCustomer.Tables["Customer"].Rows[0]["mobile_phone_number"].ToString().Trim();
        //            }
        //            if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressLine1"))
        //            {
        //                hdnMailingAddress1.Value = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine1"].ToString().Trim();
        //            }
        //            if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressPostCode"))
        //            {
        //                hdnMailingAddressPostcode.Value = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressPostCode"].ToString().Trim();
        //            }
        //        }
        //    }
        //}


        /// <summary>
        /// LoadCustomerDetails to load the email and PhoneNumber fields and other customer details
        /// </summary>
        public void LoadCustomerDetails()
        {
            int rowCount, maxRows;
            Hashtable searchData = new Hashtable();
            resultXml = string.Empty;
            conditionXML = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            searchData["CustomerID"] = Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString();
            conditionXML = Helper.HashTableToXML(searchData, "customer");
            maxRows = 100;
            DataSet dsCustomer = null;

            serviceClient = new CustomerServiceClient();

            if (serviceClient.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, maxRows, culture))
            {
                resulDoc = new XmlDocument();
                resulDoc.LoadXml(resultXml);
                dsCustomer = new DataSet();
                dsCustomer.ReadXml(new XmlNodeReader(resulDoc));
                if (dsCustomer != null)
                {
                    if (dsCustomer.Tables["Customer"].Columns.Contains("email_address"))
                    {
                        if (!string.IsNullOrEmpty(dsCustomer.Tables["Customer"].Rows[0]["email_address"].ToString().Trim())) txtBTEmail.ReadOnly = true; else txtBTEmail.ReadOnly = false;
                        txtBTEmail.Text = dsCustomer.Tables["Customer"].Rows[0]["email_address"].ToString().Trim();
                        hdnEmail.Value = dsCustomer.Tables["Customer"].Rows[0]["email_address"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("mobile_phone_number"))
                    {
                        if (!string.IsNullOrEmpty(dsCustomer.Tables["Customer"].Rows[0]["mobile_phone_number"].ToString().Trim())) txtBTMobile.ReadOnly = true; else txtBTMobile.ReadOnly = false;
                        txtBTMobile.Text = dsCustomer.Tables["Customer"].Rows[0]["mobile_phone_number"].ToString().Trim();
                        hdnSms.Value = dsCustomer.Tables["Customer"].Rows[0]["mobile_phone_number"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("TitleEnglish"))
                    {
                        hdnTiltle.Value = dsCustomer.Tables["Customer"].Rows[0]["TitleEnglish"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("Name1"))
                    {
                        hdnName1.Value = dsCustomer.Tables["Customer"].Rows[0]["Name1"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("Name2"))
                    {
                        hdnName2.Value = dsCustomer.Tables["Customer"].Rows[0]["Name2"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("Name3"))
                    {
                        hdnName3.Value = dsCustomer.Tables["Customer"].Rows[0]["Name3"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("family_member_1_dob"))
                    {
                        hdnDOB.Value = dsCustomer.Tables["Customer"].Rows[0]["family_member_1_dob"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("Sex"))
                    {
                        hdnSex.Value = dsCustomer.Tables["Customer"].Rows[0]["Sex"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressLine1"))
                    {
                        hdnMailingAddress1.Value = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine1"].ToString().Trim();
                        //lbladdress.Text = hdnMailingAddress1.Value;
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressLine2"))
                    {
                        hdnMailingAddress2.Value = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine2"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressLine3"))
                    {
                        hdnMailingAddress3.Value = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine3"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressLine4"))
                    {
                        hdnMailingAddress4.Value = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine4"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressLine5"))
                    {
                        hdnMailingAddress5.Value = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine5"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressLine6"))
                    {
                        hdnMailingAddress6.Value = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine6"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressPostCode"))
                    {
                        hdnMailingAddressPostcode.Value = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressPostCode"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("daytime_phone_number"))
                    {
                        hdnDayTimePhone.Value = dsCustomer.Tables["Customer"].Rows[0]["daytime_phone_number"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("evening_phone_number"))
                    {
                        hdnEveningPhone.Value = dsCustomer.Tables["Customer"].Rows[0]["evening_phone_number"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("SSN"))
                    {
                        hdnSSN.Value = dsCustomer.Tables["Customer"].Rows[0]["SSN"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("PassportNo"))
                    {
                        hdnPassport.Value = dsCustomer.Tables["Customer"].Rows[0]["PassportNo"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("RaceID"))
                    {
                        hdnRaceID.Value = dsCustomer.Tables["Customer"].Rows[0]["RaceID"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("ISOLanguageCode"))
                    {
                        hdnLanguage.Value = dsCustomer.Tables["Customer"].Rows[0]["ISOLanguageCode"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("CustomerMailStatus"))
                    {
                        hdnCustomerMailStatus.Value = dsCustomer.Tables["Customer"].Rows[0]["CustomerMailStatus"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("CustomerMobilePhoneStatus"))
                    {
                        hdnCustomerMobilePhoneStatus.Value = dsCustomer.Tables["Customer"].Rows[0]["CustomerMobilePhoneStatus"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("CustomerEmailStatus"))
                    {
                        hdnCustomerEmailStatus.Value = dsCustomer.Tables["Customer"].Rows[0]["CustomerEmailStatus"].ToString().Trim();
                    }
                    if (dsCustomer.Tables["Customer"].Columns.Contains("CustomerUseStatus"))
                    {
                        hdnCustomerUseStatusMain.Value = dsCustomer.Tables["Customer"].Rows[0]["CustomerUseStatus"].ToString().Trim();
                    }
                    if (dsCustomer.Tables.Contains("FamilyDetails") && dsCustomer.Tables["FamilyDetails"].Rows.Count > 0)
                    {
                        if (dsCustomer.Tables["FamilyDetails"].Rows[0]["number_of_household_members"].ToString() != "0")
                        {
                            hdnTotalHouseHoldMembers.Value = dsCustomer.Tables["FamilyDetails"].Rows[0]["number_of_household_members"].ToString().Trim();
                        }

                        if (dsCustomer.Tables["FamilyDetails"].Columns.Contains("DateOfBirth")) //Set family Age details
                        {
                            for (int i = 0; i < dsCustomer.Tables["FamilyDetails"].Rows.Count; i++)
                            {
                                if (i == 0)
                                {
                                    hdnAge1.Value = GetAge(Convert.ToDateTime(dsCustomer.Tables["FamilyDetails"].Rows[i]["DateOfBirth"])).ToString();
                                }
                                else if (i == 1)
                                {
                                    hdnAge2.Value = GetAge(Convert.ToDateTime(dsCustomer.Tables["FamilyDetails"].Rows[i]["DateOfBirth"])).ToString();
                                }
                                else if (i == 2)
                                {
                                    hdnAge3.Value = GetAge(Convert.ToDateTime(dsCustomer.Tables["FamilyDetails"].Rows[i]["DateOfBirth"])).ToString();
                                }
                                else if (i == 3)
                                {
                                    hdnAge4.Value = GetAge(Convert.ToDateTime(dsCustomer.Tables["FamilyDetails"].Rows[i]["DateOfBirth"])).ToString();
                                }
                                else
                                {
                                    hdnAge5.Value = GetAge(Convert.ToDateTime(dsCustomer.Tables["FamilyDetails"].Rows[i]["DateOfBirth"])).ToString();
                                }

                            }

                        }
                    }
                    else if (dsCustomer.Tables["NoOFFamilyMembers"] != null)
                    {
                        hdnTotalHouseHoldMembers.Value = dsCustomer.Tables["NoOFFamilyMembers"].Rows[0]["number_of_household_members"].ToString().Trim();
                    }
                }
            }
        }
        #region Dynamic Age Boxes


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

        #endregion
        public void ClearSelection()
        {
            chkVirginAtlantic.Checked = false;
            chkBAviosStandard.Checked = false;
            chkAviosPremium.Checked = false;
            chkBAviosPremium.Checked = false;
            chkAviosStandard.Checked = false;
            chkXmasSaver.Checked = false;
            txtBAvios.Text = "";
            txtAviosMembership.Text = "";
            txtVirgnMembershipID.Text = "";
        }

        ///// <summary>
        ///// LoadConfigDetails to load configuration values like prefix and max/min length of mobile field and also defaultconfig details from
        ///// Configuration table Configuartion type 3,9,5
        ///// </summary>
        //public void LoadConfigDetails()
        //{
        //    string mobileNoMinValue = string.Empty;
        //    string mobileNoMaxValue = string.Empty;
        //    string conditionXML = string.Empty;
        //    DataSet dsConfigDetails = new DataSet();
        //    int rowCount;

        //    resultXml = string.Empty;
        //    conditionXML = "3";
        //    errorXml = string.Empty;
        //    rowCount = 0;
        //    customerObj = new CustomerServiceClient();

        //    if (customerObj.GetConfigDetails(out errorXml, out resultXml, out rowCount, conditionXML, culture))
        //    {
        //        resulDoc = new XmlDocument();
        //        resulDoc.LoadXml(resultXml);
        //        dsConfigDetails.ReadXml(new XmlNodeReader(resulDoc));
        //        if (dsConfigDetails.Tables.Count > 0)
        //        {
        //            foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
        //            {
        //                if (dr["ConfigurationType"].ToString().Trim() == "3")
        //                {
        //                    sdefaultpreferenceType = dr["ConfigurationName"].ToString().Trim();
        //                }
        //            }
        //        }
        //    }
        //}


        protected void RemoveBtn_OnClick(object sender, EventArgs e)
        {
            LinkButton clickedButton = sender as LinkButton;
            GridViewRow row = (GridViewRow)clickedButton.Parent.Parent;
            int rowID = Convert.ToInt16(row.RowIndex);

            DataTable dtBTDetails = new DataTable();
            dtBTDetails.Columns.Add("DateOfBirth", typeof(string), null);
            dtBTDetails.Columns.Add("OriginalDateOfBirth", typeof(string), null);
            dtBTDetails.Columns.Add("MediaID", typeof(Int16), null);
            dtBTDetails.Columns.Add("ClubID", typeof(Int16), null);
            dtBTDetails.Columns.Add("MembershipID", typeof(string), null);
            dtBTDetails.Columns.Add("IsDeleted", typeof(string), null);
            for (int i = 0; i < grdBabyTodlerOptIn.Rows.Count; i++)
            {
                if (i != 0)
                {
                    if (i != grdBabyTodlerOptIn.Rows.Count)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerOptIn.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtBTDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                    else
                    {
                        dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                    }
                }
                else
                {
                    if (grdBabyTodlerOptIn.Rows.Count > 0)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerOptIn.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtBTDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                    else
                    {
                        dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                    }
                }
            }
            dtBTDetails.Rows[rowID].Delete();
            dtBTDetails.AcceptChanges();
            grdBabyTodlerOptIn.DataSource = dtBTDetails;
            grdBabyTodlerOptIn.DataBind();
            ViewState["BTDetails"] = dtBTDetails;
            if (dtBTDetails.Rows.Count < 5)
            {
                lnkAddChild.Enabled = true;
            }
            if (grdBabyTodlerOptIn.Rows.Count == 0)
            {
                grdBabyTodlerOptIn.Attributes.Add("style", "display:none");
            }
            else
            {
                grdBabyTodlerOptIn.Attributes.Add("style", "display:block");
            }
        }
        protected void RemoveBtnOptOut_OnClick(object sender, EventArgs e)
        {
            LinkButton clickedButton = sender as LinkButton;
            GridViewRow row = (GridViewRow)clickedButton.Parent.Parent;
            int rowID = Convert.ToInt16(row.RowIndex);

            DataTable dtBTDetails = new DataTable();
            dtBTDetails.Columns.Add("DateOfBirth", typeof(string), null);
            dtBTDetails.Columns.Add("OriginalDateOfBirth", typeof(string), null);
            dtBTDetails.Columns.Add("MediaID", typeof(Int16), null);
            dtBTDetails.Columns.Add("ClubID", typeof(Int16), null);
            dtBTDetails.Columns.Add("MembershipID", typeof(string), null);
            dtBTDetails.Columns.Add("IsDeleted", typeof(string), null);
            for (int i = 0; i < grdBabyTodlerOptOut.Rows.Count; i++)
            {
                if (i != 0)
                {
                    if (i != grdBabyTodlerOptOut.Rows.Count)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerOptOut.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtBTDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                }
                else
                {
                    if (grdBabyTodlerOptOut.Rows.Count > 0)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerOptOut.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtBTDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                    else
                    {
                        dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                    }
                }
            }
            dtBTDetails.Rows[rowID].Delete();
            dtBTDetails.AcceptChanges();
            grdBabyTodlerOptOut.DataSource = dtBTDetails;
            grdBabyTodlerOptOut.DataBind();
            ViewState["BTDetails"] = dtBTDetails;
            if (dtBTDetails.Rows.Count < 5)
            {
                LinkButton2.Enabled = true;
            }
            if (grdBabyTodlerOptOut.Rows.Count == 0)
            {
                grdBabyTodlerOptOut.Attributes.Add("style", "display:none");
            }
        }
        /// <summary>
        /// LoadBabtTodlerDetails to load the Baby Todler Club Data
        /// </summary>

        protected void grdBabyTodlerOptIn_ItemDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:Preferences.Home.grdBabyTodlerOptIn_ItemDataBound");
                NGCTrace.NGCTrace.TraceDebug("Start:Preferences.Home.grdBabyTodlerOptIn_ItemDataBound  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

                string regDate = @"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$";

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Literal ltrChildNumber = (Literal)e.Row.Cells[0].FindControl("ltrChildNumber");
                    TextBox txtDOB = (TextBox)e.Row.Cells[1].FindControl("txtDOB");

                    if (ltrChildNumber != null)
                    {
                        if (iChildNumberIn == 0)
                        {
                            iChildNumberIn = 1;
                        }
                        ltrChildNumber.Text = iChildNumberIn.ToString();
                        iChildNumberIn++;
                    }
                    if (txtDOB != null && txtDOB.Text != "")
                    {
                        if (!Helper.IsRegexMatch(txtDOB.Text.Trim(), regDate, false, false))
                        {
                            txtDOB.Text = txtDOB.Text.Trim();
                        }
                        else
                        {
                            DateTime date;
                            date = DateTime.Parse(txtDOB.Text.Trim());
                            txtDOB.Text = date.ToShortDateString();
                        }
                    }
                }
                NGCTrace.NGCTrace.TraceInfo("End:Preferences.Home.grdBabyTodlerOptIn_ItemDataBound");
                NGCTrace.NGCTrace.TraceDebug("End:Preferences.Home.grdBabyTodlerOptIn_ItemDataBound  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

            }
            catch (Exception exp)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Preferences.Home.grdBabyTodlerOptIn_ItemDataBound - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Preferences.Home.grdBabyTodlerOptIn_ItemDataBound - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Preferences.Home.grdBabyTodlerOptIn_ItemDataBound");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;
            }
        }
        protected void grdBabyTodlerOptOut_ItemDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:Preferences.Home.grdBabyTodlerOptOut_ItemDataBound");
                NGCTrace.NGCTrace.TraceDebug("Start:Preferences.Home.grdBabyTodlerOptOut_ItemDataBound  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

                string regDate = @"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$";

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Literal ltrChildNumber = (Literal)e.Row.Cells[0].FindControl("ltrChildNumber");
                    TextBox txtDOB = (TextBox)e.Row.Cells[1].FindControl("txtDOB");

                    if (ltrChildNumber != null)
                    {
                        if (iChildNumberOut == 0)
                        {
                            iChildNumberOut = 1;
                        }
                        ltrChildNumber.Text = iChildNumberOut.ToString();
                        iChildNumberOut++;
                    }
                    if (txtDOB != null && txtDOB.Text != "")
                    {
                        if (!Helper.IsRegexMatch(txtDOB.Text.Trim(), regDate, false, false))
                        {
                            txtDOB.Text = txtDOB.Text.Trim();
                        }
                        else
                        {
                            DateTime date;
                            date = DateTime.Parse(txtDOB.Text.Trim());
                            txtDOB.Text = date.ToShortDateString();
                        }
                    }
                }
                NGCTrace.NGCTrace.TraceInfo("End:Preferences.Home.grdBabyTodlerOptOut_ItemDataBound");
                NGCTrace.NGCTrace.TraceDebug("End:Preferences.Home.grdBabyTodlerOptOut_ItemDataBound  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

            }
            catch (Exception exp)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Preferences.Home.grdBabyTodlerOptOut_ItemDataBound - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Preferences.Home.grdBabyTodlerOptOut_ItemDataBound - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Preferences.Home.grdBabyTodlerOptOut_ItemDataBound");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;
            }
        }
        protected void AddChildBtn_OnClick(object sender, EventArgs e)
        {
            grdBabyTodlerOptIn.Attributes.Add("style", "display:block");

            DataTable dtBTDetails = new DataTable();
            dtBTDetails.Columns.Add("DateOfBirth", typeof(string), null);
            dtBTDetails.Columns.Add("OriginalDateOfBirth", typeof(string), null);
            dtBTDetails.Columns.Add("MediaID", typeof(Int16), null);
            dtBTDetails.Columns.Add("ClubID", typeof(Int16), null);
            dtBTDetails.Columns.Add("MembershipID", typeof(string), null);
            dtBTDetails.Columns.Add("IsDeleted", typeof(string), null);
            for (int i = 0; i <= grdBabyTodlerOptIn.Rows.Count; i++)
            {
                if (i != 0)
                {
                    if (i != grdBabyTodlerOptIn.Rows.Count)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerOptIn.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtBTDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                    else
                    {
                        dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                    }
                }
                else
                {
                    if (grdBabyTodlerOptIn.Rows.Count > 0)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerOptIn.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtBTDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                    else
                    {
                        dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                    }
                }
            }

            dtBTDetails.AcceptChanges();
            grdBabyTodlerOptIn.DataSource = dtBTDetails;
            grdBabyTodlerOptIn.DataBind();
            ViewState["BTDetails"] = dtBTDetails;
            if (dtBTDetails.Rows.Count >= 5)
            {
                lnkAddChild.Enabled = false;
                lnkAddChild.Attributes.Add("style", "cursor:auto");
            }
        }
        protected void AddBTOptOutChildBtn_OnClick(object sender, EventArgs e)
        {
            grdBabyTodlerOptOut.Attributes.Add("style", "display:block");
            DataTable dtBTDetails = new DataTable();
            dtBTDetails.Columns.Add("DateOfBirth", typeof(string), null);
            dtBTDetails.Columns.Add("OriginalDateOfBirth", typeof(string), null);
            dtBTDetails.Columns.Add("MediaID", typeof(Int16), null);
            dtBTDetails.Columns.Add("ClubID", typeof(Int16), null);
            dtBTDetails.Columns.Add("MembershipID", typeof(string), null);
            dtBTDetails.Columns.Add("IsDeleted", typeof(string), null);
            for (int i = 0; i <= grdBabyTodlerOptOut.Rows.Count; i++)
            {
                if (i != 0)
                {
                    if (i != grdBabyTodlerOptOut.Rows.Count)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerOptOut.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtBTDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                    else
                    {
                        dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                    }
                }
                else
                {
                    if (grdBabyTodlerOptOut.Rows.Count > 0)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerOptOut.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtBTDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                    else
                    {
                        dtBTDetails.Rows.Add(null, null, null, null, "", "N");
                    }
                }
            }

            dtBTDetails.AcceptChanges();
            grdBabyTodlerOptOut.DataSource = dtBTDetails;
            grdBabyTodlerOptOut.DataBind();
            ViewState["BTDetails"] = dtBTDetails;
            if (dtBTDetails.Rows.Count >= 5)
            {
                LinkButton2.Enabled = false;
                LinkButton2.Attributes.Add("style", "cursor:auto");
            }
        }


        /// <summary>
        ///  Associate 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RemoveAssoBtn_OnClick(object sender, EventArgs e)
        {
            LinkButton clickedButton = sender as LinkButton;
            GridViewRow row = (GridViewRow)clickedButton.Parent.Parent;
            int rowID = Convert.ToInt16(row.RowIndex);

            DataTable dtBTAssoDetails = new DataTable();
            dtBTAssoDetails.Columns.Add("DateOfBirth", typeof(string), null);
            dtBTAssoDetails.Columns.Add("OriginalDateOfBirth", typeof(string), null);
            dtBTAssoDetails.Columns.Add("MediaID", typeof(Int16), null);
            dtBTAssoDetails.Columns.Add("ClubID", typeof(Int16), null);
            dtBTAssoDetails.Columns.Add("MembershipID", typeof(string), null);
            dtBTAssoDetails.Columns.Add("IsDeleted", typeof(string), null);
            for (int i = 0; i < grdBabyTodlerAssoOptIn.Rows.Count; i++)
            {
                if (i != 0)
                {
                    if (i != grdBabyTodlerAssoOptIn.Rows.Count)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerAssoOptIn.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtBTAssoDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtBTAssoDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                    else
                    {
                        dtBTAssoDetails.Rows.Add(null, null, null, null, "", "N");
                    }
                }
                else
                {
                    if (grdBabyTodlerAssoOptIn.Rows.Count > 0)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerAssoOptIn.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtBTAssoDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtBTAssoDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                    else
                    {
                        dtBTAssoDetails.Rows.Add(null, null, null, null, "", "N");
                    }
                }
            }
            dtBTAssoDetails.Rows[rowID].Delete();
            dtBTAssoDetails.AcceptChanges();
            grdBabyTodlerAssoOptIn.DataSource = dtBTAssoDetails;
            grdBabyTodlerAssoOptIn.DataBind();
            ViewState["BTAssoDetails"] = dtBTAssoDetails;
            if (dtBTAssoDetails.Rows.Count < 5)
            {
                LinkButton1.Enabled = true;
            }
            if (grdBabyTodlerAssoOptIn.Rows.Count == 0)
            {
                grdBabyTodlerAssoOptIn.Attributes.Add("style", "display:none");
            }
            else
            {
                grdBabyTodlerAssoOptIn.Attributes.Add("style", "display:block");
            }
        }
        protected void RemoveBtnAssoOptOut_OnClick(object sender, EventArgs e)
        {
            LinkButton clickedButton = sender as LinkButton;
            GridViewRow row = (GridViewRow)clickedButton.Parent.Parent;
            int rowID = Convert.ToInt16(row.RowIndex);

            DataTable dtBTAssoDetails = new DataTable();
            dtBTAssoDetails.Columns.Add("DateOfBirth", typeof(string), null);
            dtBTAssoDetails.Columns.Add("OriginalDateOfBirth", typeof(string), null);
            dtBTAssoDetails.Columns.Add("MediaID", typeof(Int16), null);
            dtBTAssoDetails.Columns.Add("ClubID", typeof(Int16), null);
            dtBTAssoDetails.Columns.Add("MembershipID", typeof(string), null);
            dtBTAssoDetails.Columns.Add("IsDeleted", typeof(string), null);
            for (int i = 0; i < grdBabyTodlerAssoOptOut.Rows.Count; i++)
            {
                if (i != 0)
                {
                    if (i != grdBabyTodlerAssoOptOut.Rows.Count)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerAssoOptOut.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtBTAssoDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtBTAssoDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                }
                else
                {
                    if (grdBabyTodlerAssoOptOut.Rows.Count > 0)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerAssoOptOut.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtBTAssoDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtBTAssoDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                    else
                    {
                        dtBTAssoDetails.Rows.Add(null, null, null, null, "", "N");
                    }
                }
            }
            dtBTAssoDetails.Rows[rowID].Delete();
            dtBTAssoDetails.AcceptChanges();
            grdBabyTodlerAssoOptOut.DataSource = dtBTAssoDetails;
            grdBabyTodlerAssoOptOut.DataBind();
            ViewState["BTAssoDetails"] = dtBTAssoDetails;
            if (dtBTAssoDetails.Rows.Count < 5)
            {
                LinkButton3.Enabled = true;
            }
            if (grdBabyTodlerAssoOptOut.Rows.Count == 0)
            {
                grdBabyTodlerAssoOptOut.Attributes.Add("style", "display:none");
            }
        }
        /// <summary>
        /// LoadBabtTodlerDetails to load the Baby Todler Club Data
        /// </summary>

        protected void grdBabyTodlerAssoOptIn_ItemDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:Preferences.Home.grdBabyTodlerOptIn_ItemDataBound");
                NGCTrace.NGCTrace.TraceDebug("Start:Preferences.Home.grdBabyTodlerOptIn_ItemDataBound  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

                string regDate = @"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$";

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Literal ltrChildNumber = (Literal)e.Row.Cells[0].FindControl("ltrChildNumber");
                    TextBox txtDOB = (TextBox)e.Row.Cells[1].FindControl("txtDOB");

                    if (ltrChildNumber != null)
                    {
                        if (iChildAssoNumberIn == 0)
                        {
                            iChildAssoNumberIn = 1;
                        }
                        ltrChildNumber.Text = iChildAssoNumberIn.ToString();
                        iChildAssoNumberIn++;
                    }
                    if (txtDOB != null && txtDOB.Text != "")
                    {
                        if (!Helper.IsRegexMatch(txtDOB.Text.Trim(), regDate, false, false))
                        {
                            txtDOB.Text = txtDOB.Text.Trim();
                        }
                        else
                        {
                            DateTime date;
                            date = DateTime.Parse(txtDOB.Text.Trim());
                            txtDOB.Text = date.ToShortDateString();
                        }
                    }
                }
                NGCTrace.NGCTrace.TraceInfo("End:Preferences.Home.grdBabyTodlerOptIn_ItemDataBound");
                NGCTrace.NGCTrace.TraceDebug("End:Preferences.Home.grdBabyTodlerOptIn_ItemDataBound  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

            }
            catch (Exception exp)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Preferences.Home.grdBabyTodlerOptIn_ItemDataBound - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Preferences.Home.grdBabyTodlerOptIn_ItemDataBound - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Preferences.Home.grdBabyTodlerOptIn_ItemDataBound");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;
            }
        }
        protected void grdBabyTodlerAssoOptOut_ItemDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:Preferences.Home.grdBabyTodlerOptOut_ItemDataBound");
                NGCTrace.NGCTrace.TraceDebug("Start:Preferences.Home.grdBabyTodlerOptOut_ItemDataBound  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

                string regDate = @"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$";

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Literal ltrChildNumber = (Literal)e.Row.Cells[0].FindControl("ltrChildNumber");
                    TextBox txtDOB = (TextBox)e.Row.Cells[1].FindControl("txtDOB");

                    if (ltrChildNumber != null)
                    {
                        if (iChildAssoNumberOut == 0)
                        {
                            iChildAssoNumberOut = 1;
                        }
                        ltrChildNumber.Text = iChildAssoNumberOut.ToString();
                        iChildAssoNumberOut++;
                    }
                    if (txtDOB != null && txtDOB.Text != "")
                    {
                        if (!Helper.IsRegexMatch(txtDOB.Text.Trim(), regDate, false, false))
                        {
                            txtDOB.Text = txtDOB.Text.Trim();
                        }
                        else
                        {
                            DateTime date;
                            date = DateTime.Parse(txtDOB.Text.Trim());
                            txtDOB.Text = date.ToShortDateString();
                        }
                    }
                }
                NGCTrace.NGCTrace.TraceInfo("End:Preferences.Home.grdBabyTodlerOptOut_ItemDataBound");
                NGCTrace.NGCTrace.TraceDebug("End:Preferences.Home.grdBabyTodlerOptOut_ItemDataBound  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

            }
            catch (Exception exp)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Preferences.Home.grdBabyTodlerOptOut_ItemDataBound - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Preferences.Home.grdBabyTodlerOptOut_ItemDataBound - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Preferences.Home.grdBabyTodlerOptOut_ItemDataBound");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;
            }
        }
        protected void AddChildAssoBtn_OnClick(object sender, EventArgs e)
        {
            grdBabyTodlerAssoOptIn.Attributes.Add("style", "display:block");

            DataTable dtAssoBTDetails = new DataTable();
            dtAssoBTDetails.Columns.Add("DateOfBirth", typeof(string), null);
            dtAssoBTDetails.Columns.Add("OriginalDateOfBirth", typeof(string), null);
            dtAssoBTDetails.Columns.Add("MediaID", typeof(Int16), null);
            dtAssoBTDetails.Columns.Add("ClubID", typeof(Int16), null);
            dtAssoBTDetails.Columns.Add("MembershipID", typeof(string), null);
            dtAssoBTDetails.Columns.Add("IsDeleted", typeof(string), null);
            for (int i = 0; i <= grdBabyTodlerAssoOptIn.Rows.Count; i++)
            {
                if (i != 0)
                {
                    if (i != grdBabyTodlerAssoOptIn.Rows.Count)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerAssoOptIn.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtAssoBTDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtAssoBTDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                    else
                    {
                        dtAssoBTDetails.Rows.Add(null, null, null, null, "", "N");
                    }
                }
                else
                {
                    if (grdBabyTodlerAssoOptIn.Rows.Count > 0)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerAssoOptIn.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtAssoBTDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtAssoBTDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                    else
                    {
                        dtAssoBTDetails.Rows.Add(null, null, null, null, "", "N");
                    }
                }
            }

            dtAssoBTDetails.AcceptChanges();
            grdBabyTodlerAssoOptIn.DataSource = dtAssoBTDetails;
            grdBabyTodlerAssoOptIn.DataBind();
            ViewState["BTAssoDetails"] = dtAssoBTDetails;
            if (dtAssoBTDetails.Rows.Count >= 5)
            {
                LinkButton1.Enabled = false;
                LinkButton1.Attributes.Add("style", "cursor:auto");
            }
        }
        protected void AddBTOptOutAssoChildBtn_OnClick(object sender, EventArgs e)
        {
            grdBabyTodlerAssoOptOut.Attributes.Add("style", "display:block");
            DataTable dtAssoBTDetails = new DataTable();
            dtAssoBTDetails.Columns.Add("DateOfBirth", typeof(string), null);
            dtAssoBTDetails.Columns.Add("OriginalDateOfBirth", typeof(string), null);
            dtAssoBTDetails.Columns.Add("MediaID", typeof(Int16), null);
            dtAssoBTDetails.Columns.Add("ClubID", typeof(Int16), null);
            dtAssoBTDetails.Columns.Add("MembershipID", typeof(string), null);
            dtAssoBTDetails.Columns.Add("IsDeleted", typeof(string), null);
            for (int i = 0; i <= grdBabyTodlerAssoOptOut.Rows.Count; i++)
            {
                if (i != 0)
                {
                    if (i != grdBabyTodlerAssoOptOut.Rows.Count)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerAssoOptOut.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtAssoBTDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtAssoBTDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                    else
                    {
                        dtAssoBTDetails.Rows.Add(null, null, null, null, "", "N");
                    }
                }
                else
                {
                    if (grdBabyTodlerAssoOptOut.Rows.Count > 0)
                    {
                        TextBox txtDOB = (TextBox)grdBabyTodlerAssoOptOut.Rows[i].Cells[1].FindControl("txtDOB");
                        if (txtDOB.Text.Trim() != "")
                        {
                            dtAssoBTDetails.Rows.Add(null, txtDOB.Text.Trim(), null, null, "", "N");
                        }
                        else
                        {
                            dtAssoBTDetails.Rows.Add(null, null, null, null, "", "N");
                        }
                    }
                    else
                    {
                        dtAssoBTDetails.Rows.Add(null, null, null, null, "", "N");
                    }
                }
            }

            dtAssoBTDetails.AcceptChanges();
            grdBabyTodlerAssoOptOut.DataSource = dtAssoBTDetails;
            grdBabyTodlerAssoOptOut.DataBind();
            ViewState["BTAssoDetails"] = dtAssoBTDetails;
            if (dtAssoBTDetails.Rows.Count >= 5)
            {
                LinkButton3.Enabled = false;
                LinkButton3.Attributes.Add("style", "cursor:auto");
            }
        }

        protected void ResetHiddenValues()
        {
            hdnPref43.Value = "false";
            hdnPref44.Value = "false";
            hdnPref45.Value = "false";
            hdnPref48.Value = "false";
            hdnPref6.Value = "false";
            hdnPref7.Value = "false";
            hdnPref8.Value = "false";
            hdnPref10.Value = "false";
            hdnPref12.Value = "false";
            hdnPref13.Value = "false";
            hdnPref15.Value = "false";
            hdnPref16.Value = "false";
            hdnPref17.Value = "false";
            hdnPref27.Value = "false";
            hdnPref28.Value = "false";
            hdnPref29.Value = "false";
            hdnPref30.Value = "false";
            hdnPref31.Value = "false";
            hdnPref32.Value = "false";
            hdnPref33.Value = "false";
            hdnPref34.Value = "false";
            hdnPref35.Value = "false";
            hdnPref36.Value = "false";
            hdnPref37.Value = "false";
            hdnPref38.Value = "false";
            //LCM changes
            hdnPref39.Value = "false";
            hdnPref40.Value = "false";
            hdnPref41.Value = "false";
            hdnPref42.Value = "false";
            //LCM changes
        }
    }
}
