using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using CCODundeeApplication.CustomerService;
using System.ServiceModel;
using System.DirectoryServices;
using System.EnterpriseServices;
using System.DirectoryServices.ActiveDirectory;

namespace CCODundeeApplication
{
    public partial class AddUser : System.Web.UI.Page
    {
        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        protected CustomerServiceClient customerClient = null;
        Hashtable htCustomer = null;
        string culture = string.Empty;
        string domain = string.Empty;
        DataSet dsGroups = null;
        protected string errMsgUserName = string.Empty;
        protected string spanUserName = "display:none";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                Label lblViewPoints = (Label)Master.FindControl("lblViewPoints");
                lblViewPoints.Visible = false;
                Label lblresetpass = (Label)Master.FindControl("lblresetpass");
                lblresetpass.Visible = false;
                Label lblDelinking = (Label)Master.FindControl("lblDelinking");
                lblDelinking.Visible = false;
                Label lblDataConfiguration = (Label)Master.FindControl("lblDataConfiguration");
                lblDataConfiguration.Visible = false;
                Label lblCustomerCoupons = (Label)Master.FindControl("lblCustomerCoupons");
                lblCustomerCoupons.Visible = false;
                //Added as a part of Group CR phase CR12
                Label lblUserNotes = (Label)Master.FindControl("lblUserNotes");
                lblUserNotes.Visible = false;
           
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


                    }
                }
                #endregion
                GetGroupDetails();
            }

            lblSuccessMessage.Text = "";
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
        protected void btnValidate_Click(object sender, EventArgs e)
        {
            if (ValidatePage())
            {
                string DisplayName = ValidateActiveDirectoryUser(txtUserName.Text);


                try
                {
                    #region Trace Start
                    NGCTrace.NGCTrace.TraceInfo("Start: CSC Add User.btnValidate_Click() Display Name-" + DisplayName);
                    NGCTrace.NGCTrace.TraceDebug("Start: CSC Add User.btnValidate_Click() Culture-" + ConfigurationManager.AppSettings["Domain"].ToString() + "Display Name-" + DisplayName);
                    #endregion

                    if (ConfigurationManager.AppSettings["LDAPAddUserSplit"] == "commaseparator")
                    {
                        if (DisplayName != null && DisplayName != string.Empty)
                        {
                            string[] Name = DisplayName.Split(',');
                            if (Name.Length >= 0)
                            {

                                txtLastName.Text = DisplayName.Split(',')[0].ToString().Trim();
                                btnAdd.Enabled = true;
                            }
                            if (Name.Length > 0)
                            {
                                txtFirstName.Text = DisplayName.Split(',')[1].ToString().Trim();
                            }
                        }
                        else
                        {
                            lblSuccessMessage.Text = GetLocalResourceObject("UserName.Valid").ToString();
                            txtFirstName.Text = string.Empty;
                            txtLastName.Text = string.Empty;
                        }
                    }
                    else if (ConfigurationManager.AppSettings["LDAPAddUserSplit"] == "space")
                    {
                        if (DisplayName != null && DisplayName != string.Empty)
                        {

                            string[] Name = DisplayName.Split(' ');
                            txtFirstName.Text = Name[0].ToString().Trim();
                            txtLastName.Text = Name[1].ToString().Trim();
                            btnAdd.Enabled = true;
                        }
                        else
                        {
                            lblSuccessMessage.Text = GetLocalResourceObject("UserName.Valid").ToString();
                            txtFirstName.Text = string.Empty;
                            txtLastName.Text = string.Empty;
                        }

                    }

                    #region Trace End
                    NGCTrace.NGCTrace.TraceInfo("End: CSC Add User.btnValidate_Click() Display Name-" + DisplayName);
                    NGCTrace.NGCTrace.TraceDebug("End: CSC Add User.btnValidate_Click() Culture-" + ConfigurationManager.AppSettings["Domain"].ToString() + "Display Name-" + DisplayName);
                    #endregion

                }
                catch (Exception exp)
                {
                    #region Trace Error
                    NGCTrace.NGCTrace.TraceCritical("Critical: CSC ADD USER.btnValidate_Click()- Error Message :" + exp.ToString());
                    NGCTrace.NGCTrace.TraceError("Error: CSC ADD USER.btnValidate_Click()  - Error Message :" + exp.ToString());
                    NGCTrace.NGCTrace.TraceWarning("Warning: CSC ADD USER.btnValidate_Click()");
                    NGCTrace.NGCTrace.ExeptionHandling(exp);
                    #endregion Trace Error
                    throw exp;
                }
                finally
                {
                }
            }

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                customerClient = new CustomerServiceClient();
                long objectid = 0;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                htCustomer = new Hashtable();
                htCustomer["UserName"] = txtUserName.Text;
                htCustomer["UserDescription"] = txtFirstName.Text + " " + txtLastName.Text;
                htCustomer["UserStatusCode"] = "1";
                htCustomer["EmailAddress"] = string.Empty;
                htCustomer["UserID"] = 0;
                if (ValidateAddbtn())
                {
                    htCustomer["RoleID"] = ddlGroups.SelectedValue.ToString();

                    string addUserXml = Helper.HashTableToXML(htCustomer, "ApplicationUser");

                    #region Trace Start
                    NGCTrace.NGCTrace.TraceInfo("Start: CSC Add User.btnAdd_Click()");
                    NGCTrace.NGCTrace.TraceDebug("Start: CSC Add User..btnAdd_Click() Input Xml-" + addUserXml);
                    #endregion

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                    {
                        if (customerClient.Add(out objectid, out resultXml, out errorXml, addUserXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                        {
                            if (resultXml.Contains("UniqueConstraint UserName"))
                                lblSuccessMessage.Text = GetLocalResourceObject("UniqueConsMsg1.Text").ToString() + txtUserName.Text + GetLocalResourceObject("UniqueConsMsg2.Text").ToString();
                            //"User " + txtUserName.Text + " Already Exists.";
                            else
                            {
                                lblSuccessMessage.Text = GetLocalResourceObject("lblSuccessMessage.Text").ToString();//"User been saved successfuly.";
                                txtFirstName.Text = string.Empty;
                                txtLastName.Text = string.Empty;
                                txtUserName.Text = string.Empty;
                                ddlGroups.SelectedIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }
                

                #region Trace End
                    NGCTrace.NGCTrace.TraceInfo("End: CSC Add User..btnAdd_Click()");
                    NGCTrace.NGCTrace.TraceDebug("End: CSC Add User..btnAdd_Click() Input Xml-" + addUserXml);
                #endregion
            }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ADD USER.btnAdd_Click()- Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC ADD USER.btnAdd_Click()  - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ADD USER.btnAdd_Click()");
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

        /// <summary>
        /// Fetch All Groups From DataBase
        /// </summary>
        public void GetGroupDetails()
        {
            string addresses = string.Empty;
            string addressDetails = string.Empty;
            culture = ConfigurationManager.AppSettings["Culture"].ToString();
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            XmlDocument resulDoc = null;
            try
            {
                customerClient = new CustomerServiceClient();
                dsGroups = new DataSet();
                htCustomer = new Hashtable();
                htCustomer["UserID"] = 0;
                string insertxml = Helper.HashTableToXML(htCustomer, "ROLE");

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ADD USER.GetGroupDetails()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ADD USER.GetGroupDetails()");
                #endregion

                if (customerClient.GetGroupDetails(out errorXml, out resultXml, insertxml, culture))
                {
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsGroups.ReadXml(new XmlNodeReader(resulDoc));
                    FillGroupDropDown(dsGroups);
                }
                else
                {
                }

                #region Trace END
                NGCTrace.NGCTrace.TraceInfo("End: CSC ADD USER.GetGroupDetails()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC ADD USER.GetGroupDetails()");
                #endregion
            }

            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ADD USER.GetGroupDetails()- Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC ADD USER.GetGroupDetails()  - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ADD USER.GetGroupDetails()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

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

        #region Fill Groups Drop Down

        public void FillGroupDropDown(DataSet ds)
        {

            DataRow selectRow = ds.Tables[0].NewRow();
            selectRow["RoleNameEnglish"] = "Select"; // add a 'select' row on the top of the table
            ds.Tables[0].Rows.InsertAt(selectRow, 0);
            ddlGroups.DataMember = "RoleNameEnglish";
            ddlGroups.DataTextField = "RoleNameEnglish";
            ddlGroups.DataValueField = "RoleID";
            ddlGroups.DataSource = ds.Tables[0];
            ddlGroups.DataBind();
        }
        #endregion

        #region Validate User With Active Directory

        /// <summary>
        /// Function To Validate User With Active Directory
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>User Details as First And Last Name</returns>
        protected string ValidateActiveDirectoryUser(string userName)
        {
            String loginStatus = string.Empty;
            String strLDAPPath = string.Empty;
            String LDAPUserName = string.Empty;
            String LDAPPassword = string.Empty;

            try
            {

                strLDAPPath = Convert.ToString(ConfigurationSettings.AppSettings["LDAPPathIN"]);
                LDAPUserName = Convert.ToString(ConfigurationSettings.AppSettings["LDAPUserId"]);
                LDAPPassword = Convert.ToString(ConfigurationSettings.AppSettings["LDAPPassword"]);
                DirectoryEntry objDirEntry = new DirectoryEntry(strLDAPPath, LDAPUserName, LDAPPassword);
                DirectorySearcher search = new DirectorySearcher(objDirEntry);


                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ADD USER.ValidateActiveDirectoryUser() LDAP PATH -" + strLDAPPath + " UserName-" + LDAPUserName + " Pswd-" + LDAPPassword);
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ADD USER.ValidateActiveDirectoryUser() LDAP PATH -" + strLDAPPath + " UserName-" + LDAPUserName + " Pswd-" + LDAPPassword);
                #endregion


                search.Filter = "(SAMAccountName=" + userName + ")";
                SearchResult result;
                result = search.FindOne();
                if (result != null)
                {
                    DirectoryEntry directoryEntry = result.GetDirectoryEntry();
                    string displayName = directoryEntry.Properties["displayName"][0].ToString();
                    if (result.Path == "")
                    {
                        loginStatus = null;

                    }
                    else
                    {
                        loginStatus = displayName;

                    }
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC ADD USER.ValidateActiveDirectoryUser() LDAP PATH -" + strLDAPPath + " UserName-" + LDAPUserName + " Pswd-" + LDAPPassword);
                NGCTrace.NGCTrace.TraceDebug("End: CSC ADD USER.ValidateActiveDirectoryUser() LDAP PATH -" + strLDAPPath + " UserName-" + LDAPUserName + " Pswd-" + LDAPPassword);
                #endregion
            }
            catch (Exception exp)
            {

                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ADD USER.ValidateActiveDirectoryUser() LDAP PATH -" + strLDAPPath + " UserName-" + LDAPUserName + " Pswd-" + LDAPPassword + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC ADD USER.ValidateActiveDirectoryUser() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ADD USER.ValidateActiveDirectoryUser()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
            }
            return loginStatus;
        }

        #endregion

        /// <summary>
        /// To validate customer details
        /// </summary>
        /// <returns>boolean</returns>
        protected bool ValidatePage()
        {
            try
            {

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ADD USER.ValidatePage()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ADD USER.ValidatePage()");
                #endregion
                //string regNumeric = @"^[0-9]*$";
                bool bErrorFlag = true;

                //Clear the class
                txtUserName.CssClass = "";

                string userName = txtUserName.Text.ToString().Trim();

                //Server side validations
                if (string.IsNullOrEmpty(userName))
                {
                    spnErrorMsg.Visible = true;
                    spnErrorMsg.Attributes.Add("Style", "display:block");
                    bErrorFlag = false;
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC ADD USER.ValidatePage()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC ADD USER.ValidatePage()");
                #endregion

                return bErrorFlag;

            }
            catch (Exception exp)
            {

                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ADD USER.ValidatePage()- Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC ADD USER.ValidatePage() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ADD USER.ValidatePage()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;

            }
        }

            public bool ValidateAddbtn()
            {
                try
            {

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ADD USER.ValidatePage()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ADD USER.ValidatePage()");
                #endregion
                lblSuccessMessage.Text = "";
                //string regNumeric = @"^[0-9]*$";
                bool bErrorFlag = true;
                
                if (ddlGroups.SelectedItem.Text.ToString() == "Select")
                {
                    lblSuccessMessage.Text = "Please select Valid GroupName";
                    bErrorFlag = false;
                }
                else
                    bErrorFlag = true;
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC ADD USER.ValidatePage()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC ADD USER.ValidatePage()");
                #endregion

                return bErrorFlag;

            }
                catch (Exception exp)
            {

                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ADD USER.ValidatePage()- Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC ADD USER.ValidatePage() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ADD USER.ValidatePage()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;

            }
            }

        }
    }

