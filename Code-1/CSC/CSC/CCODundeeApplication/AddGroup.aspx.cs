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
using CCODundeeApplication.AdminService;
using System.ServiceModel;
using System.DirectoryServices;
using System.EnterpriseServices;
using System.DirectoryServices.ActiveDirectory;


namespace CCODundeeApplication
{
    public partial class AddGroup : System.Web.UI.Page
    {

        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        protected CustomerServiceClient customerClient = null;
        protected AdminServiceClient adminClient = null;
        Hashtable htCustomer = null;
        string culture = string.Empty;
        string domain = string.Empty;
        DataSet dsGroups = null;
        protected string errMsgGroupName = string.Empty;
        protected string spanGroupName = "display:none";
        protected string spanValidName = "display:none";
        protected string errMsgvalid = string.Empty;
        protected string errMsgvalidDes = string.Empty;
        protected string spanValidDesc = "display:none";
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            if (ValidatePage())
            {
                try
                {
                    adminClient = new AdminServiceClient();
                    long objectid = 0;
                    string resultXml = string.Empty;
                    string errorXml = string.Empty;
                    htCustomer = new Hashtable();
                    htCustomer["RoleName"] = txtGroupName.Text;
                    htCustomer["RoleDesc"] = txtDescription.Text;
                    string addGroupXml = Helper.HashTableToXML(htCustomer, "Role");

                    #region Trace Start
                    NGCTrace.NGCTrace.TraceInfo("Start: CSC ADD Group.btn_Add_Click().AddRole");
                    NGCTrace.NGCTrace.TraceDebug("Start: CSC ADD Group.btn_Add_Click().AddRole RoleName :" + txtGroupName.Text + " RoleDesc" + txtDescription.Text);
                    #endregion

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                    {

                        if (adminClient.AddRole(out objectid, out resultXml, addGroupXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                        {
                            if (resultXml.Contains("UniqueConstraint RoleName"))
                                //lblSuccessMessage.Text = "Group " + txtGroupName.Text + " Already Exists.";
                                lblSuccessMessage.Text = GetLocalResourceObject("UniqueConsMsg1.Text").ToString() + txtGroupName.Text + GetLocalResourceObject("UniqueConsMsg2.Text").ToString();
                            else
                            {
                                lblSuccessMessage.Text = GetLocalResourceObject("SuccessMsg.Text").ToString();
                                // "Group has been added successfuly.";
                                txtGroupName.Text = string.Empty;
                                txtDescription.Text = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }

                    #region Trace END
                    NGCTrace.NGCTrace.TraceInfo("END: CSC ADD Group.btn_Add_Click().AddRole");
                    NGCTrace.NGCTrace.TraceDebug("END: CSC ADD Group.btn_Add_Click().AddRole RoleName :" + txtGroupName.Text + " RoleDesc" + txtDescription.Text);
                    #endregion
                }
                catch (Exception exp)
                {
                    //Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                    //    "Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                    //throw exp;

                    #region Trace Error
                    NGCTrace.NGCTrace.TraceCritical("Critical: CSC ADD Group.btn_Add_Click().AddRole RoleName :" + txtGroupName.Text + " RoleDesc" + txtDescription.Text + "- Error Message :" + exp.ToString());
                    NGCTrace.NGCTrace.TraceError("Error: CSC ADD Group.btn_Add_Click().AddRole RoleName :" + txtGroupName.Text + " RoleDesc" + txtDescription.Text + " - Error Message :" + exp.ToString());
                    NGCTrace.NGCTrace.TraceWarning("Warning: CSC ADD Group.btn_Add_Click().AddRole");
                    NGCTrace.NGCTrace.ExeptionHandling(exp);
                    #endregion Trace Error


                }
                finally
                {
                    if (adminClient != null)
                    {
                        if (adminClient.State == CommunicationState.Faulted)
                        {
                            adminClient.Abort();
                        }
                        else if (adminClient.State != CommunicationState.Closed)
                        {
                            adminClient.Close();
                        }
                    }
                }

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
                bool bErrorFlag = true;

                //Clear the class
                txtGroupName.CssClass = "";

                string userName = txtGroupName.Text.ToString().Trim();

                //Server side validations


                if (string.IsNullOrEmpty(userName))
                {
                    spnErrorMsg.Visible = true;
                    spnErrorMsg.Attributes.Add("Style", "display:block");
                    bErrorFlag = false;
                    //errMsgGroupName = "Please enter a Group Name";
                    //spanGroupName = "";
                    //txtGroupName.CssClass = "errorFld";
                    //bErrorFlag = false;
                }


                return bErrorFlag;

            }
            catch (Exception exp)
            {
                //Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                //    "Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                //throw exp;
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ADD Group.btn_Add_Click().AddRole RoleName :" + txtGroupName.Text + " RoleDesc" + txtDescription.Text + "- Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC ADD Group.btn_Add_Click().AddRole RoleName :" + txtGroupName.Text + " RoleDesc" + txtDescription.Text + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ADD Group.btn_Add_Click().AddRole");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

    }
}
