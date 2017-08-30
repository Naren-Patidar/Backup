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
using CCODundeeApplication.CustomerService;
using System.ServiceModel;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CCODundeeApplication
{
    public partial class Join : System.Web.UI.Page
    {
        #region Local varibales

        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        Hashtable htCustomer = null;
        CustomerServiceClient customerClient = null;
        //Used in .aspx page for for hiding/unhiding the controls
        protected string spanCardNumber = "display:none";
        protected string spanLastName = "display:none";
        protected string spanFirstName = "display:none";
        protected string spanEmailAddress = "display:none";

        protected string errMsgCardNumber = string.Empty;
        protected string errMsgFirstName = string.Empty;
        protected string errMsgLastName = string.Empty;
        protected string errMsgEmailAddress = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            txtCardNumber.Focus();
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
                Label lblCustomerCoupons = (Label)Master.FindControl("lblCustomerCoupons");
                lblCustomerCoupons.Visible = false;
                Label lblDataConfiguration = (Label)Master.FindControl("lblDataConfiguration");
                lblDataConfiguration.Visible = false;
                //Added as a part of Group CR phase CR12
                Label lblUserNotes = (Label)Master.FindControl("lblUserNotes");
                lblUserNotes.Visible = false;
                GetConfigDetails();
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
                        HtmlAnchor DataConfig = (HtmlAnchor)Master.FindControl("dataconfig");
                        HtmlAnchor AccountOperationReports = (HtmlAnchor)Master.FindControl("AccReports");
                        HtmlAnchor PromotionalCodeReport = (HtmlAnchor)Master.FindControl("PromotionalCode");
                        Control link = (HtmlGenericControl)Master.FindControl("liDataconfig");
                        HtmlAnchor PointsearnedReport = (HtmlAnchor)Master.FindControl("PointsEarnedReport");
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
            }
        }

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

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            lblSuccessMessage.Text = string.Empty;
            if (ValidatePage())
            {
                if (CalculateCheckDigit(txtCardNumber.Text.ToString()))
                {
                    try
                    {
                        customerClient = new CustomerServiceClient();
                        string resultXml = string.Empty;
                        string errorXml = string.Empty;
                        htCustomer = new Hashtable();
                        htCustomer["cardAccountNumber"] = txtCardNumber.Text.Trim();
                        htCustomer["Fstname"] = txtFirstName.Text.Trim();
                        htCustomer["Lstname"] = txtLastName.Text.Trim();
                        htCustomer["Emailid"] = txtEmailAddress.Text.Trim();
                        string addUserXml = Helper.HashTableToXML(htCustomer, "AddCustomer");

                        #region Trace Start
                        NGCTrace.NGCTrace.TraceInfo("Start: CSC FindUser.btnConfirm_Click()");
                        NGCTrace.NGCTrace.TraceDebug("Start: CSC FindUser.btnConfirm_Click() Input Xml-" + addUserXml);
                        #endregion

                        if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                        {
                            resultXml = string.Empty;

                            if (customerClient.AddCustomer(out resultXml, out errorXml, addUserXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                            {

                                if (resultXml.Contains("Customer Already Exists"))
                                {
                                    //Code Modified for Localization.
                                    //case 1//UniqueConsMsg1.Text//GetLocalResourceObject("UniqueConsMsg1.Text").ToString()
                                    //lblSuccessMessage.Text = "Customer already registered with details";
                                    lblSuccessMessage.Text = GetLocalResourceObject("UniqueConsMsg1.Text").ToString();
                                }
                                else if (resultXml.Contains("Card has been added successfully"))
                                {
                                    //Code Modified for Localization.
                                    //case 4//UniqueConsMsg2.Text
                                    //lblSuccessMessage.Text = "Card has been added successfully";
                                    lblSuccessMessage.Text = GetLocalResourceObject("UniqueConsMsg2.Text").ToString();
                                }
                                else if (resultXml.Contains("Maximun of three active cards can be linked to the customer"))
                                {
                                    //Code Modified for Localization.
                                    //case 4//UniqueConsMsg3.Text
                                    //lblSuccessMessage.Text = "Maximum of three active cards can be linked to the customer";
                                    lblSuccessMessage.Text = GetLocalResourceObject("UniqueConsMsg3.Text").ToString();
                                }
                                else if (resultXml.Contains("Merged successfully"))
                                {
                                    //Code Modified for Localization.
                                    //Case 5//UniqueConsMsg4.Text
                                    //lblSuccessMessage.Text = "Customer details has been saved successfully";
                                    lblSuccessMessage.Text = GetLocalResourceObject("UniqueConsMsg4.Text").ToString();
                                }
                                else if (resultXml.Contains("Invalid card range"))
                                {
                                    //Code Modified for Localization.
                                    //case Invalid Range//UniqueConsMsg5.Text
                                    lblSuccessMessage.Text = string.Empty;
                                    // errMsgCardNumber = "Invalid card range";
                                    errMsgCardNumber = GetLocalResourceObject("UniqueConsMsg5.Text").ToString();
                                    spanCardNumber = "";
                                    txtCardNumber.CssClass = "errorFld";
                                }
                                else
                                {

                                    string body = ConfigurationSettings.AppSettings["ResetPswdLink"].ToString() + CreateSecuredToken(txtEmailAddress.Text.ToString().Trim());
                                    customerClient = new CustomerServiceClient();
                                    customerClient.SendEmailET(txtEmailAddress.Text.Trim(), body, ConfigurationManager.AppSettings["ActivationEmail"].ToString());
                                    //Code Modified for Localization.
                                    //Case 2 n Case 3//UniqueConsMsg6.Text
                                    //lblSuccessMessage.Text = "Email has been sent successfully";
                                    lblSuccessMessage.Text = GetLocalResourceObject("UniqueConsMsg6.Text").ToString();
                                    txtCardNumber.Text = string.Empty;
                                    txtEmailAddress.Text = string.Empty;
                                    txtFirstName.Text = string.Empty;
                                    txtLastName.Text = string.Empty;


                                }
                            }
                        }
                        else
                        {
                            Response.Redirect("~/Default.aspx", false);
                        }
                        #region Trace End
                        NGCTrace.NGCTrace.TraceInfo("End: CSC FindUser.btnConfirm_Click()");
                        NGCTrace.NGCTrace.TraceDebug("End: CSC FindUser.btnConfirm_Click() Input Xml-" + addUserXml);
                        #endregion

                    }
                    catch (Exception exp)
                    {
                        #region Trace Error
                        NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.ImageButton1_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                        NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.ImageButton1_Click() - Error Message :" + exp.ToString());
                        NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.ImageButton1_Click()");
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
                    //Code Modified for Localization.
                    //Resource file id:UniqueConsMsg7.Text
                    //errMsgCardNumber = "Please enter a valid Card Number";
                    errMsgCardNumber = GetLocalResourceObject("UniqueConsMsg7.Text").ToString();
                    spanCardNumber = "";
                    txtCardNumber.CssClass = "errorFld";
                }
            }
        }

        #region CreateToken
        public string CreateSecuredToken(string userName)
        {

            string tokenId = string.Empty;
            customerClient = new CustomerServiceClient();
            try
            {
                string resultXml = string.Empty;
                bool tstatus = customerClient.CreateToken(out resultXml, userName);
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindUser.CreateSecuredToken()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindUser.CreateSecuredToken()");
                #endregion
                if (tstatus)
                {

                    XmlDocument resultDoc = new XmlDocument();
                    resultDoc.LoadXml(resultXml);
                    DataSet dsCustomer = new DataSet();
                    dsCustomer.ReadXml(new XmlNodeReader(resultDoc));
                    if (dsCustomer.Tables[0].Rows.Count > 0)
                    {
                        tokenId = dsCustomer.Tables[0].Rows[0]["TokenId"].ToString();
                    }

                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindUser.CreateSecuredToken()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindUser.CreateSecuredToken()");
                #endregion

            }

            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.CreateSecuredToken() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.CreateSecuredToken() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.CreateSecuredToken()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }

            finally

            { customerClient = null; }

            return tokenId;

        }


        #endregion

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

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindUser.ValidatePage()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindUser.ValidatePage()");
                #endregion

                //string regNumeric = @"^[0-9]*$";
                //string regFirstName = @"^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*$";
                //string regSurName = @"^[a-zA-Z]+(([\'\.\- ][a-zA-Z ])?[a-zA-Z]*)*$";
                //string regMail = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

                string regNumeric = hdnNumericeg.Value;
                string regFirstName=hdnname1reg.Value;
                string regSurName=hdnname3reg.Value;
                string regMail=hdnemailreg.Value;
                bool bErrorFlag = true;

                //Clear the class
                txtFirstName.CssClass = "";
                txtLastName.CssClass = "";
                txtEmailAddress.CssClass = "";
                txtCardNumber.CssClass = "";

                string firstName = txtFirstName.Text.ToString().Trim();
                string lastName = txtLastName.Text.ToString().Trim();
                string cardNumber = txtCardNumber.Text.ToString().Trim();
                string emailAddress = txtEmailAddress.Text.ToString().Trim();

                //Server side validations
                if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(emailAddress))
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
                            //Code Modified for Localization.
                            //Resource file id:UniqueConsMsg7.Text
                            //errMsgCardNumber = "Please enter a valid Card Number";
                            errMsgCardNumber = GetLocalResourceObject("UniqueConsMsg7.Text").ToString();
                            spanCardNumber = "";
                            txtCardNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                        //Card number should be more between 16  and 18 digits
                        else if (!string.IsNullOrEmpty(txtCardNumber.Text.Trim()) && (txtCardNumber.Text.Trim().Length < 16 || txtCardNumber.Text.Trim().Length > 18))
                        {
                            //Code Modified for Localization.
                            //Resource file id:UniqueConsMsg7.Text
                            //errMsgCardNumber = "Please enter a valid Card Number";
                            errMsgCardNumber = GetLocalResourceObject("UniqueConsMsg7.Text").ToString();
                            spanCardNumber = "";
                            txtCardNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                        //Card number should not be all zeros.
                        else if (!string.IsNullOrEmpty(txtCardNumber.Text.Trim()) && Convert.ToInt64(txtCardNumber.Text) == 0)
                        {
                            //Code Modified for Localization.
                            //Resource file id:UniqueConsMsg7.Text
                            //errMsgCardNumber = "Please enter a valid Card Number";
                            errMsgCardNumber = GetLocalResourceObject("UniqueConsMsg7.Text").ToString();
                            spanCardNumber = "";
                            txtCardNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }
                    catch (FormatException)
                    {
                        //Code Modified for Localization.
                        //Resource file id:UniqueConsMsg7.Text
                        //errMsgCardNumber = "Please enter a valid Card Number";
                        errMsgCardNumber = GetLocalResourceObject("UniqueConsMsg7.Text").ToString();
                        spanCardNumber = "";
                        txtCardNumber.CssClass = "errorFld";
                        bErrorFlag = false;
                    }


                    //First Name
                    if (!Helper.IsRegexMatch(txtFirstName.Text.Trim(), regFirstName, true, false))
                    {
                        //Code Modified for Localization.
                        //UniqueConsMsg8.Text
                        //errMsgFirstName = "Please enter a valid Name";
                        errMsgFirstName = GetLocalResourceObject("UniqueConsMsg8.Text").ToString();
                        spanFirstName = "";
                        txtFirstName.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                    //Last Name
                    if (!Helper.IsRegexMatch(txtLastName.Text.Trim(), regSurName, true, false))
                    {
                        //Code Modified for Localization.
                        //UniqueConsMsg8.Text
                        //errMsgLastName = "Please enter a valid Name";
                        errMsgLastName = GetLocalResourceObject("UniqueConsMsg8.Text").ToString();
                        spanLastName = "";
                        txtLastName.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                    //Card Number
                    if (!Helper.IsRegexMatch(txtCardNumber.Text.Trim(), regNumeric, true, false))
                    {
                        //Code Modified for Localization.
                        //Resource file id:UniqueConsMsg7.Text
                        //errMsgCardNumber = "Please enter a valid Card Number";
                        errMsgCardNumber = GetLocalResourceObject("UniqueConsMsg7.Text").ToString();
                        spanCardNumber = "";
                        txtCardNumber.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                    //Email Address
                    if (!Helper.IsRegexMatch(txtEmailAddress.Text.Trim(), regMail, true, false))
                    {
                        //Code Modified for Localization.
                        //UniqueConsMsg9.Text
                        //errMsgEmailAddress = "Please enter a valid Email Address";
                        errMsgEmailAddress = GetLocalResourceObject("UniqueConsMsg9.Text").ToString();
                        spanEmailAddress = "";
                        txtEmailAddress.CssClass = "errorFld";
                        bErrorFlag = false;
                    }


                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindUser.ValidatePage()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindUser.ValidatePage()");
                #endregion
                return bErrorFlag;
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.ValidatePage() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.ValidatePage() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.ValidatePage()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        private void GetConfigDetails()
        {
            customerClient = new CustomerServiceClient();
            string culture = ConfigurationManager.AppSettings["Culture"].ToString();
            string conditionConfigXML = "10";
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            int rowCount = 0;
            XmlDocument resulDoc = null;
            DataSet dsConfigDetails = new DataSet();
            if (customerClient.GetConfigDetails(out errorXml, out resultXml, out rowCount, conditionConfigXML, Culture))
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
                        if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Numeric")
                        {
                            hdnNumericeg.Value = dr["ConfigurationValue1"].ToString();
                        }
                        else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "EmailAddress")
                        {
                            hdnemailreg.Value = dr["ConfigurationValue1"].ToString();
                        }
                        else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Name1")
                        {
                            hdnname1reg.Value = dr["ConfigurationValue1"].ToString();
                        }
                        else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Name3")
                        {
                            hdnname3reg.Value = dr["ConfigurationValue1"].ToString();
                        }

                    }
                }
            }
        }
    }
}
