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
using System.Threading;
using System.Globalization;
using System.Xml;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CCODundeeApplication
{
    public partial class DeLinking : System.Web.UI.Page
    {
        #region LocalVariables
        private long customerID = 0;
        string culture = ConfigurationManager.AppSettings["Culture"];
        protected CustomerService.CustomerServiceClient customerObj = null;
        protected ClubcardService.ClubcardServiceClient clubcardObj = null;
        Hashtable searchData = null;
        DataSet dsCustomer = null;
        XmlDocument resulDoc = null;
        string conditionXML = string.Empty;
        string resultXml = string.Empty;
        string errorXml = string.Empty;
        XmlDocument xmlCapability = null;
        DataSet dsCapability = null;
        // string DelConfirmMsg = GetLocalResourceObject("DeleteConfirmMessage").ToString();

        //DataSet dsCustomerHouseholdStatus = null;
        //int rowCount = 0;
        #endregion


        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC DeLinking.Page_Load() CustomerID" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                NGCTrace.NGCTrace.TraceDebug("Start: CSC DeLinking.Page_Load() CustomerID" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                #endregion

                if (!IsPostBack && !string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID")))
                {

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                    {
                        #region RoleCapabilityImplementation
                        xmlCapability = new XmlDocument();
                        dsCapability = new DataSet();
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

                            HtmlAnchor customerCoupon = (HtmlAnchor)Master.FindControl("customerCoupon");
                            HtmlAnchor CardRange = (HtmlAnchor)Master.FindControl("CardRange");
                            HtmlAnchor CardTypes = (HtmlAnchor)Master.FindControl("CardType");
                            HtmlAnchor Stores = (HtmlAnchor)Master.FindControl("Stores");
                            Label DeLinkAccount = (Label)Master.FindControl("lblDelinking");
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

                            }

                            if (dsCapability.Tables[0].Columns.Contains("editdelinkaccounts") != false)
                            {
                                GridForAssociative.Enabled = true;
                                grdMainCustomer.Enabled = true;
                                hdnMEdit.Value = "true";
                            }
                            else
                            {
                                GridForAssociative.Enabled = false;
                                grdMainCustomer.Enabled = false;
                                hdnMEdit.Value = "false";
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
                            if (dsCapability.Tables[0].Columns.Contains("ViewCustomerDetails") != false)
                            {
                                cutomerDetails.Disabled = false;
                            }
                            else
                            {
                                cutomerDetails.Disabled = true;
                                cutomerDetails.HRef = "";
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


                    }


                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID")))
                    {
                        customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                        clubcardObj = new ClubcardService.ClubcardServiceClient();

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

                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }

                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC DeLinking.Page_Load() CustomerID" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                NGCTrace.NGCTrace.TraceDebug("End: CSC DeLinking.Page_Load() CustomerID" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC DeLinking.Page_Load()- Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC DeLinking.Page_Load() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC DeLinking.Page_Load()");
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
                    }
                    else if (((Convert.ToInt64(customer["PrimaryCustomerID"].ToString()) == loggedInCustomerID) && isAssociate == false))
                    {
                        customerID = customer["CustomerID"].ToString();
                        rowNumber = 1;
                        hdnAssociateCustID.Value = customerID;
                        isAssociate = true;
                        isThirdCustomer = false;
                    }

                    //Check if already Main and Associate customers displayed.
                    if (!isThirdCustomer)
                    {
                        searchData["CustomerID"] = customerID;

                        //Preparing parameters for service call
                        conditionXML = Helper.HashTableToXML(searchData, "customer");
                        maxRows = 100;
                        customerObj = new CustomerService.CustomerServiceClient();

                        if (customerObj.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, maxRows, Culture))
                        {
                            resulDoc = new XmlDocument();
                            resulDoc.LoadXml(resultXml);
                            dsCustomer = new DataSet();
                            dsCustomer.ReadXml(new XmlNodeReader(resulDoc));

                            DateTime dob = DateTime.Now;

                            //For main customer
                            if (rowNumber == 0)
                            {
                                if (dsCustomer.Tables["Customer"].Columns.Contains("TitleEnglish"))
                                    txtTitle0.Text = Helper.ToTitleCase(dsCustomer.Tables["Customer"].Rows[0]["TitleEnglish"].ToString().Trim());

                                if (dsCustomer.Tables["Customer"].Columns.Contains("Name1")) txtFirstName0.Text = Helper.ToTitleCase(dsCustomer.Tables["Customer"].Rows[0]["Name1"].ToString().Trim());

                                if (ConfigurationManager.AppSettings["Culture"].ToString() == "en-GB")
                                {
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("Name2")) txtInitial0.Text = dsCustomer.Tables["Customer"].Rows[0]["Name2"].ToString().Trim();
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("Name3")) txtSurname0.Text = Helper.ToTitleCase(dsCustomer.Tables["Customer"].Rows[0]["Name3"].ToString().Trim());
                                }
                                else
                                {
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("Name3")) txtInitial0.Text = Helper.ToTitleCase(dsCustomer.Tables["Customer"].Rows[0]["Name3"].ToString().Trim());
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("Name2")) txtSurname0.Text = Helper.ToTitleCase(dsCustomer.Tables["Customer"].Rows[0]["Name2"].ToString().Trim());
                                }
                                if (hdnPrimaryCustID.Value != null)
                                {
                                    LoadDataFromService_PrdAlternativeIdsSummaryForMainCustomer();
                                }


                            }
                            //For associate customer customer
                            else if (rowNumber == 1)
                            {
                                if (dsCustomer.Tables["Customer"].Columns.Contains("TitleEnglish"))
                                {
                                    txtTitleAss.Text = Helper.ToTitleCase(dsCustomer.Tables["Customer"].Rows[0]["TitleEnglish"].ToString().Trim());
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
                                //Assign household customer count to hidden field.
                                hdnNumberOfCustomers.Value = numberOfCustomers.ToString();
                                if (hdnAssociateCustID.Value != null)
                                {
                                    LoadDataFromService_PrdAlternativeIdsSummaryForAssociativeCustomer();
                                }
                            }
                        }
                    }
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

        #endregion

        #region GridbindingMethods


        private void LoadDataFromService_PrdAlternativeIdsSummaryForMainCustomer()
        {
            #region Local variables
            string conditionXml, resultXml, viewXml = string.Empty, errorXml;
            int maxRowCount = 0, rowCount = 0;
            Hashtable inputParams = new Hashtable();
            XmlDocument resulDoc = new XmlDocument();
            DataSet dsAlternativeDotcomIds = new DataSet();
            string culture = string.Empty;
            bool isSuccessful = false;
            //ClubcardService.ClubcardServiceClient serviceClient = null;
            CustomerService.CustomerServiceClient serviceClient = null;
            #endregion

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC DeLinking.LoadDataFromService_PrvPrdPointsSummary()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC DeLinking.LoadDataFromService_PrvPrdPointsSummary()");
                #endregion
                //Check ViewState first if the dataset is present in ViewState
                //Initialize it from ViewState and bypass the NGC service call
                if (ViewState["dsPrvPrdPointsSummary"] != null)
                {
                    dsAlternativeDotcomIds = ViewState["dsPrvPrdPointsSummary"] as DataSet;
                }
                else
                {
                    //Initialize the service reference
                    serviceClient = new CustomerService.CustomerServiceClient();

                    inputParams["CustomerID"] = hdnPrimaryCustID.Value; //selected customerID
                    culture = ConfigurationManager.AppSettings["Culture"];

                    //Convert all input variables to xml
                    conditionXml = Helper.HashTableToXML(inputParams, "CustomerAlternateID");
                    //public bool GetAlternativeIds(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
                    //public bool GetAlternativeIds(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
                    isSuccessful = serviceClient.GetAlternativeIds(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture);
                    //If service is successful load the xml into the dsPointsSummaryRec dataset
                    if (isSuccessful && string.IsNullOrEmpty(errorXml))
                    {
                        if (!string.IsNullOrEmpty(resultXml))
                        {
                            //Load the result xml containing parameters into a data set
                            resulDoc.LoadXml(resultXml);
                            dsAlternativeDotcomIds.ReadXml(new XmlNodeReader(resulDoc));
                        }
                        if (dsAlternativeDotcomIds != null &&
                            dsAlternativeDotcomIds.Tables.Count > 0)
                        {
                            //Save the dataset to view state for postback cycles
                            ViewState["dsAlternativeDotcomIds"] = dsAlternativeDotcomIds;
                            grdMainCustomer.DataSource = dsAlternativeDotcomIds.Tables[0].DefaultView;
                            grdMainCustomer.DataBind();

                        }
                        else
                        {
                            grdMainCustomer.DataSource = null;
                            grdMainCustomer.DataBind();
                            dvMainAlterCustomer.Visible = false;

                        }
                    }
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC DeLinking.LoadDataFromService_PrvPrdPointsSummary()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC DeLinking.LoadDataFromService_PrvPrdPointsSummary()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC DeLinking.LoadDataFromService_PrvPrdPointsSummary() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC DeLinking.LoadDataFromService_PrvPrdPointsSummary() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC DeLinking.LoadDataFromService_PrvPrdPointsSummary()");
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



        private void LoadDataFromService_PrdAlternativeIdsSummaryForAssociativeCustomer()
        {
            #region Local variables
            string conditionXml, resultXml, viewXml = string.Empty, errorXml;
            int maxRowCount = 0, rowCount = 0;
            Hashtable inputParams = new Hashtable();
            XmlDocument resulDoc = new XmlDocument();
            DataSet dsAlternativeDotcomIdsForAssociative = new DataSet();
            string culture = string.Empty;
            bool isSuccessful = false;
            //ClubcardService.ClubcardServiceClient serviceClient = null;
            CustomerService.CustomerServiceClient serviceClient = null;
            #endregion

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC DeLinking.LoadDataFromService_PrvPrdPointsSummary()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC DeLinking.LoadDataFromService_PrvPrdPointsSummary()");
                #endregion
                //Check ViewState first if the dataset is present in ViewState
                //Initialize it from ViewState and bypass the NGC service call
                if (ViewState["dsPrvPrdPointsSummary"] != null)
                {
                    dsAlternativeDotcomIdsForAssociative = ViewState["dsPrvPrdPointsSummary"] as DataSet;
                }
                else
                {
                    //Initialize the service reference
                    serviceClient = new CustomerService.CustomerServiceClient();

                    inputParams["CustomerID"] = hdnAssociateCustID.Value; //selected customerID
                    culture = ConfigurationManager.AppSettings["Culture"];

                    //Convert all input variables to xml
                    conditionXml = Helper.HashTableToXML(inputParams, "CustomerAlternateID");
                    //public bool GetAlternativeIds(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
                    //public bool GetAlternativeIds(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
                    isSuccessful = serviceClient.GetAlternativeIds(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture);
                    //If service is successful load the xml into the dsPointsSummaryRec dataset
                    if (isSuccessful && string.IsNullOrEmpty(errorXml))
                    {
                        if (!string.IsNullOrEmpty(resultXml))
                        {
                            //Load the result xml containing parameters into a data set
                            resulDoc.LoadXml(resultXml);
                            dsAlternativeDotcomIdsForAssociative.ReadXml(new XmlNodeReader(resulDoc));
                        }
                        if (dsAlternativeDotcomIdsForAssociative != null &&
                            dsAlternativeDotcomIdsForAssociative.Tables.Count > 0)
                        {
                            //Save the dataset to view state for postback cycles
                            ViewState["dsAlternativeDotcomIds"] = dsAlternativeDotcomIdsForAssociative;
                            GridForAssociative.DataSource = dsAlternativeDotcomIdsForAssociative.Tables[0].DefaultView;
                            GridForAssociative.DataBind();
                        }
                        else
                        {
                            GridForAssociative.DataSource = null;
                            GridForAssociative.DataBind();
                            dvAssAlterCustomer.Visible = false;
                        }
                    }
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC DeLinking.LoadDataFromService_PrvPrdPointsSummary()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC DeLinking.LoadDataFromService_PrvPrdPointsSummary()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC DeLinking.LoadDataFromService_PrvPrdPointsSummary() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC DeLinking.LoadDataFromService_PrvPrdPointsSummary() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC DeLinking.LoadDataFromService_PrvPrdPointsSummary()");
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


        #region GridEvents

        protected void grdMainCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Convert.ToBoolean(hdnMEdit.Value))
                {
                    string message = string.Empty;
                    message = GetLocalResourceObject("DeleteConfirmMessage").ToString();
                    LinkButton l = (LinkButton)e.Row.FindControl("lnkDelCustomerDotcom");
                    l.Attributes.Add("onclick", "javascript:return " +
                    "confirm('" + message + "')");
                }

            }

        }


        protected void GridForAssociative_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Convert.ToBoolean(hdnMEdit.Value))
                {
                    string message = string.Empty;
                    message = GetLocalResourceObject("DeleteConfirmMessage").ToString();
                    LinkButton l = (LinkButton)e.Row.FindControl("lnkDelCustomerDotcomAss");
                    l.Attributes.Add("onclick", "javascript:return " +
                    "confirm('" + message + "')");
                }
            }

        }

        protected void GridForAssociative_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Hashtable HSDotComIds;
            string objectXml = string.Empty, resultXml;
            CustomerService.CustomerServiceClient serviceClient = null;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC DeLinking.GridForAssociative_RowCommand()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC DeLinking.GridForAssociative_RowCommand()");
                #endregion
                if (e.CommandName == "Delete")
                {
                    serviceClient = new CustomerService.CustomerServiceClient();
                    String AlternativeDotComId = e.CommandArgument.ToString();
                    HSDotComIds = new Hashtable();
                    HSDotComIds["CustomerAlternateID"] = AlternativeDotComId;
                    objectXml = Helper.HashTableToXML(HSDotComIds, "CusAlterId");
                    if (serviceClient.DeLinkingDotcomAccounts(out resultXml, objectXml))
                    {
                        LoadDataFromService_PrdAlternativeIdsSummaryForAssociativeCustomer();
                    }
                    else
                    {
                        Response.Redirect("Default.aspx", false);
                    }

                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC DeLinking.GridForAssociative_RowCommand()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC DeLinking.GridForAssociative_RowCommand()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC DeLinking.GridForAssociative_RowCommand() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC DeLinking.GridForAssociative_RowCommand() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC DeLinking.GridForAssociative_RowCommand()");
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


        protected void grdMainCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Hashtable HSDotComIds;
            string objectXml = string.Empty, resultXml;
            CustomerService.CustomerServiceClient serviceClient = null;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC DeLinking.grdMainCustomer_RowCommand()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC DeLinking.grdMainCustomer_RowCommand()");
                #endregion
                if (e.CommandName == "Delete")
                {
                    serviceClient = new CustomerService.CustomerServiceClient();
                    String AlternativeDotComId = e.CommandArgument.ToString();
                    HSDotComIds = new Hashtable();
                    HSDotComIds["CustomerAlternateID"] = AlternativeDotComId;
                    objectXml = Helper.HashTableToXML(HSDotComIds, "CusAlterId");
                    if (serviceClient.DeLinkingDotcomAccounts(out resultXml, objectXml))
                    {
                        LoadDataFromService_PrdAlternativeIdsSummaryForMainCustomer();
                    }
                    else
                    {
                        Response.Redirect("Default.aspx", false);
                    }

                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC DeLinking.grdMainCustomer_RowCommand()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC DeLinking.grdMainCustomer_RowCommand()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC DeLinking.grdMainCustomer_RowCommand() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC DeLinking.grdMainCustomer_RowCommand() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC DeLinking.grdMainCustomer_RowCommand()");
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

        protected void grdMainCustomer_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GridForAssociative_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        #endregion


    }





}

