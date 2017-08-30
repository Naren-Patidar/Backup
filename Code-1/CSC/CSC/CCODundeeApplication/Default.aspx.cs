using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using CCODundeeApplication.CustomerService;
using System.ServiceModel;


namespace CCODundeeApplication
{
    /// <summary>
    /// Description: This is the CSC login page, here CSC user 
    /// can login by providing the user id and password and the proper domain name
    /// Author: Robin Apoto
    /// Date: 20 Sept 2010
    /// </summary>
    public partial class _Default : System.Web.UI.Page
    {
        CustomerServiceClient customerObject = null;
        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        string culture = string.Empty;
        string domain = string.Empty;

        //Used in .aspx page for for hiding/unhiding the controls
        protected string spanStyleUserID = "display:none";
        protected string spanStylePassword = "display:none";

        protected void Page_Load(object sender, EventArgs e)
        {
            txtUserID.Focus();
            if (!IsPostBack)
            {
                //To default the domain
                Helper.SetTripleDESEncryptedCookie("Culture", ConfigurationSettings.AppSettings["LoginDefaultCulture"].ToString());
                BindDomainDropDown();

                // Login Image Configurable
                ImgLoginFlag.Visible = Convert.ToBoolean(ConfigurationSettings.AppSettings["ShowLoginFlag"].ToString());
                ImgLoginFlag.ImageUrl = "~/I/" + ConfigurationSettings.AppSettings["LoginFlagImageURL"].ToString();
                imgbtnLocalFlag.Visible = Convert.ToBoolean(ConfigurationSettings.AppSettings["ShowLocalizationFlag"].ToString());
                imgbtnFlag.Visible = Convert.ToBoolean(ConfigurationSettings.AppSettings["ShowLocalizationFlag"].ToString());

            }
        }

        #region Initialize the culture
        /// <summary>
        /// Set the Page theme
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Page.Theme = Session["Culture"].ToString();
        }

        /// <summary>
        /// Initialize the culture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void InitializeCulture()
        {
            string culture = (string)this.Page.Request.Params["Culture"];
            if (culture == "" || culture == null)
                culture = Convert.ToString(ConfigurationSettings.AppSettings["LoginDefaultCulture"]);
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            base.InitializeCulture();
            Helper.SetTripleDESEncryptedCookie("Culture", culture);
        }
        #endregion
        protected void Login(object sender, ImageClickEventArgs e)
        {
            string userName = string.Empty;
            string domain = string.Empty;
            string passWord = string.Empty;
            btnConfirmPersonalDtls.Focus();
            try
            {
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                int userID = 0;
                string AppName = "CSC";
                xmlCapability = new XmlDocument();
                dsCapability = new DataSet();
                userName = txtUserID.Text.ToString().Trim();
                passWord = txtPassword.Text.ToString().Trim();
                culture = ConfigurationManager.AppSettings["Culture"].ToString();
                lblAuthorization.Visible = false;
                lblAuthenticationError.Visible = false;
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC _Default.Login()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC _Default.Login() User Name :" + userName);
                #endregion

                if (ddlDomain.SelectedValue != null)
                {
                    string LDPath = ddlDomain.SelectedValue.ToString();
                    domain = ddlDomain.SelectedItem.ToString().ToLower();
                    // Attempt to connect.
                    //Connect to NGC web service and get the session id
                    customerObject = new CustomerServiceClient();
                    if (customerObject.AuthenticateUser(out errorXml, out resultXml, out userID, domain, userName, passWord, culture, AppName, LDPath))
                    {
                        if (resultXml != null && resultXml != string.Empty)
                        {
                            xmlCapability.LoadXml(resultXml);
                            dsCapability.ReadXml(new XmlNodeReader(xmlCapability));
                            if (dsCapability.Tables.Count > 0)
                            {
                                //Session["UserCapability"] = dsCapability;
                                //Cookie created
                                Helper.SetTripleDESEncryptedCookie("UserCapability", resultXml);
                                Helper.SetTripleDESEncryptedCookie("UserName", userName.Trim());
                                Helper.SetTripleDESEncryptedCookie("UserID", userID.ToString().Trim());
                                Response.Redirect("SearchCustomer.aspx", false);
                            }
                            else
                            {
                                lblAuthorization.Visible = true;
                            }
                        }
                        else
                        {
                            lblAuthenticationError.Visible = true;
                        }
                    }
                    else
                    {
                        lblAuthenticationError.Visible = true;
                        throw new Exception(errorXml);
                    }
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC _Default.Login()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC _Default.Login() User Name :" + userName);
                #endregion
            }
            catch (Exception exp)
            {

                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC _Default.Login() User Name :" + userName + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC _Default.Login() User Name :" + userName + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ADD Group.btn_Add_Click().AddRole");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                //throw exp;

            }
            finally
            {
                if (customerObject != null)
                {
                    if (customerObject.State == CommunicationState.Faulted)
                    {
                        customerObject.Abort();
                    }
                    else if (customerObject.State != CommunicationState.Closed)
                    {
                        customerObject.Close();
                    }
                }
            }
        }

        protected void imgbtnLocalFlag_Click(object sender, ImageClickEventArgs e)
        {
            string Culture = "en-GB";
            Helper.SetTripleDESEncryptedCookie("Culture", Culture);
            Response.Redirect("Default.aspx?Culture=" + Culture);
        }

        protected void imgbtnFlag_Click(object sender, ImageClickEventArgs e)
        {
            string Culture = ConfigurationSettings.AppSettings["LoginDefaultCulture"].ToString();
            Helper.SetTripleDESEncryptedCookie("Culture", Culture);
            Response.Redirect("Default.aspx?Culture=" + Culture);

        }
        protected void BindDomainDropDown()
        {
            String strDomains = ConfigurationManager.AppSettings["Domains"];
            String strLdapPath = ConfigurationManager.AppSettings["LDAPPaths"];
            //removed comma separeted value by '|' symbol 08-08-2012 Kumar P.
            String[] vdomains = strDomains.Split('|');
            String[] vldappath = strLdapPath.Split('|');

            for (int i = 0; i < vdomains.Length; i++)
            {
                ddlDomain.Items.Add(vdomains[i].ToString());
                ddlDomain.Items[i].Value = vldappath[i].ToString();

            }
        }
    }
}