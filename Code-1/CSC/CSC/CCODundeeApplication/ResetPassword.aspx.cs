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
using System.Xml;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CCODundeeApplication
{
    public partial class ResetPassword : System.Web.UI.Page
    {

        #region Member Variables
        System.Web.Security.MembershipUser membershipUser;
        long customerID;
        protected CustomerServiceClient customerClient = null;
        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        #endregion Member Variables

        #region Page Load

        /// <summary>
        /// Handles page load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                Label lblXmasSaver = (Label)Master.FindControl("lblXmasSaver");
                lblXmasSaver.Visible = false;
                Label lblViewPoints = (Label)Master.FindControl("lblViewPoints");
                lblViewPoints.Visible = false;
                Label lblresetpass = (Label)Master.FindControl("lblresetpass");
                lblresetpass.Visible = false;
                Label lblDelinking = (Label)Master.FindControl("lblDelinking");
                lblDelinking.Visible = false;
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

                GeEmailAddress();
                lblCustName.Text = Helper.CheckAndResetCookieExpiration("lblName").ToString();
                if (IsActive(hdnEmailAddress.Value.ToString().Trim()))
                {

                    string alertMessage = "Are you sure you would like to set customer account? This would unlock the account and send an authenticated link to " + hdnEmailAddress.Value.ToString().Trim();
                    imgbtnEnablePswd.Attributes.Add("onclick", "javascript:return " + "confirm('" + alertMessage + "')");
                }
                else
                {
                    imgbtnEnablePswd.Enabled = false;
                    lblCustName.Text = lblCustName.Text + " - User Account Not Confirmed";
                }
            }
            lblSuccessMessage.Text = "";
        }

        #endregion Page Load

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

        #region Private Methods

        /// <summary>
        /// Method to get email address.
        /// </summary>
        private void GeEmailAddress()
        {
            customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
            // customerID = 1000074;
            customerClient = new CustomerServiceClient();
            //String CustomerEmailId = customerClient.GetCustomerID(customerID);
            hdnEmailAddress.Value = customerClient.GetCustomerID(customerID);
        }

        #endregion Private Methods

        #region Event Handlers
        /// <summary>
        /// Handles imgbtnEnablePswd button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgbtnEnablePswd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID")))
                {
                    if (hdnEmailAddress.Value.ToString().Trim() != string.Empty)
                    {
                        System.Web.Security.MembershipUser userInfo = Membership.GetUser(hdnEmailAddress.Value.ToString().Trim());

                        #region Trace Start
                        NGCTrace.NGCTrace.TraceInfo("Start: CSC ResetPassword.imgbtnEnablePswd_Click()");
                        NGCTrace.NGCTrace.TraceDebug("Start: CSC ResetPassword.imgbtnEnablePswd_Click() userInfo-Email-" + userInfo.Email + " IsApproved - " + userInfo.IsApproved);
                        #endregion

                        if(userInfo.IsApproved)
                        {
                            userInfo.UnlockUser();
                            Membership.UpdateUser(userInfo);
                            string EmailAddress = hdnEmailAddress.Value.ToString().Trim();
                            string body = ConfigurationSettings.AppSettings["ResetPswdLink"].ToString() + CreateSecuredToken(EmailAddress);
                            customerClient = new CustomerServiceClient();
                            customerClient.SendEmailET(EmailAddress, body, ConfigurationManager.AppSettings["ForgotPwdEmail"].ToString());
                                lblSuccessMessage.Text = "Email have been sent successfully.";
                        }
                       
                    }
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC ResetPassword.imgbtnEnablePswd_Click()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC ResetPassword.imgbtnEnablePswd_Click()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ResetPassword.imgbtnEnablePswd_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC ResetPassword.imgbtnEnablePswd_Click() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ResetPassword.imgbtnEnablePswd_Click()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                lblSuccessMessage.Text = "There was an error while sending email.";
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




        #endregion Event Handlers

        #region CreateToken
        public string CreateSecuredToken(string userName)
        {

            string tokenId = string.Empty;
            customerClient = new CustomerServiceClient();
            try
            {
                string resultXml = string.Empty;

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ResetPassword.imgbtnEnablePswd_Click()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ResetPassword.imgbtnEnablePswd_Click() username-" + userName);
                #endregion

                bool tstatus = customerClient.CreateToken(out resultXml, userName);
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
                NGCTrace.NGCTrace.TraceInfo("End: CSC ResetPassword.imgbtnEnablePswd_Click()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC ResetPassword.imgbtnEnablePswd_Click() username-" + userName);
                #endregion

            }

            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ResetPassword.CreateSecuredToken() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC ResetPassword.CreateSecuredToken() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ResetPassword.CreateSecuredToken()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }

            finally

            { customerClient = null; }

            return tokenId;

        }


        #endregion

        #region IsActive

        public bool IsActive(string EmailAddress)
        {
            bool status = false;
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID")))
            {
                if (EmailAddress!= string.Empty)
                {
                    System.Web.Security.MembershipUser userInfo = Membership.GetUser(EmailAddress);
                    if (userInfo.IsApproved)
                    {
                        status = true;
                    }
                    else
                        status = false;
                }
            }
            return status;
        }
        #endregion



    }
}
       