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
using System.Globalization;
using CCODundeeApplication.PreferenceServices;


namespace CCODundeeApplication
{
    public partial class MergeCard : System.Web.UI.Page
    {
        ClubcardServiceClient serviceClient = null;
        CustomerServiceClient customerObj = null;
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
        string amendBy = string.Empty;
        string amendDateTime = string.Empty;
        string culture = ConfigurationSettings.AppSettings["Culture"].ToString();
        long clubcard;
        PreferenceServiceClient preferenceserviceClient = null;



        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Request.QueryString["MergeSuccess"]) != null)
            {
                string succmessage = Convert.ToString(Request.QueryString["MergeSuccess"]);
                if (succmessage == "Success")
                {
                    lblSuccessMessage.Text = GetLocalResourceObject("SuccMessage").ToString();

                }
            }

            if (!this.IsPostBack)
            {

                RenderCardsSectionByCustomers();


            }
        }


        protected void RenderCardsSectionByCustomers()
        {
            //string allUserNames = string.Empty;
            long customerID = 0;
            string resultXml, errorXml;
            serviceClient = new ClubcardServiceClient();

            #region Trace Start
            NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerCards.Page_Load() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
            NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerCards.Page_Load() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
            #endregion
            try
            {
                RepeaterItem clubcard = (RepeaterItem)FindControl("ltrClubcardNumber");

                customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

                if (serviceClient.GetClubcardsCustomer(out errorXml, out resultXml, customerID, culture))
                {
                    if (resultXml != "" && resultXml != "<NewDataSet />")
                    {
                        XmlDocument resulDoc = new XmlDocument();
                        resulDoc.LoadXml(resultXml);
                        DataSet dsClubcard = new DataSet();
                        dsClubcard.ReadXml(new XmlNodeReader(resulDoc));
                        hdnClubcard.Value = dsClubcard.Tables["ClubcardDetails"].Rows[0]["ClubCardID"].ToString();
                        ////Bind it to the repeater on the Cards control
                        if (dsClubcard.Tables["ClubcardDetails"] != null)
                        {
                            rptCardDetails.DataSource = dsClubcard.Tables["ClubcardDetails"];
                            rptCardDetails.DataBind();
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
                                               string houseHoldID, string currentPoints, string joinDate, string JoinRouteCode, string PromotionalCode, long customerID, string amendBy, string amendDateTime)
        {
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            DataSet dsCustomerPreference = null;
            string customerName = string.Empty;
            DateTime joinedDate;

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
                Helper.SetTripleDESEncryptedCookie("lblCustomerLastAmendDate", amendDateTime);
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
                Label lblUserNotes = (Label)Master.FindControl("lblUserNotes");
                lblUserNotes.Visible = true;
                //******** Group CR phase1 CR12 ******** 


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

        protected void FindCustomer(object sender, EventArgs e)
        {

            DataSet dsCustomerInfo = null;
            XmlDocument resulDoc = null;
            Hashtable searchData = null;
            string conditionXml = string.Empty;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            int rowCount, maxRows;
            maxRows = 0;
            customerObj = new CustomerServiceClient();
            //For pagination
            lblSuccessMessage.Text = "";


            try
            {
                if (ValidatePage())
                {
                    searchData = new Hashtable();

                    if (txtCardNumber.Text != string.Empty) searchData["cardAccountNumber"] = Convert.ToInt64(txtCardNumber.Text);
                    searchData["CustomerID"] = 0;
                    //searchData["Culture"] = ConfigurationManager.AppSettings["Culture"].ToString();
                    //Preparing parameters for service call
                    conditionXml = Helper.HashTableToXML(searchData, "customer");

                    maxRows = 200;


                    // customerObj = new CustomerServiceClient();
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
                        //ViewState["dsCustomerInfo"] = dsCustomerInfo;
                    }
                    else
                    {
                        throw new Exception("Search Customer failed for condition: " + conditionXml + "; errorXml: " + errorXml);
                    }



                    if (dsCustomerInfo.Tables.Count > 0)
                    {
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
                        if (dsCustomerInfo.Tables["Customer"].Rows.Count == 1)
                        {
                            string customerName = string.Empty;
                            DateTime joinedDate;

                            hdnTitle.Value = dsCustomerInfo.Tables["Customer"].Rows[0]["TitleEnglish"].ToString().Trim();
                            hdnFname.Value = dsCustomerInfo.Tables["Customer"].Rows[0]["Name1"].ToString().Trim();
                            hdnMname.Value = dsCustomerInfo.Tables["Customer"].Rows[0]["Name2"].ToString().Trim();
                            hdnLname.Value = dsCustomerInfo.Tables["Customer"].Rows[0]["Name3"].ToString().Trim();

                            //Added as a part of Group CR phase CR12
                            hdnamendBy.Value = dsCustomerInfo.Tables["Customer"].Rows[0]["AmendBy"].ToString().Trim();
                            hdnamendDateTime.Value = dsCustomerInfo.Tables["Customer"].Rows[0]["AmendDateTime"].ToString().Trim();
                            //******* Group CR phase1 CR12

                            hdncardNumber.Value = dsCustomerInfo.Tables["Customer"].Rows[0]["ClubcardID"].ToString().Trim();
                            hdnhouseHoldID.Value = dsCustomerInfo.Tables["Customer"].Rows[0]["HouseHoldID"].ToString().Trim();

                            if (dsCustomerInfo.Tables["Customer"].Rows[0]["CurrentPointsBalanceQty"].ToString().Trim() == string.Empty)
                            {
                                hdncurrentPoints.Value = "0";
                            }
                            else
                            {
                                hdncurrentPoints.Value = dsCustomerInfo.Tables["Customer"].Rows[0]["CurrentPointsBalanceQty"].ToString().Trim();
                            }

                            if (DateTime.TryParse(dsCustomerInfo.Tables["Customer"].Rows[0]["JoinedDate"].ToString().Trim(), out joinedDate))
                            {
                                hdnjoinDate.Value = joinedDate.ToString("dd/MM/yy");
                            }
                            if (!string.IsNullOrEmpty(dsCustomerInfo.Tables["Customer"].Rows[0]["JoinRouteDesc"].ToString().Trim()))
                            {
                                hdnJoinRouteCode.Value = dsCustomerInfo.Tables["Customer"].Rows[0]["JoinRouteDesc"].ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(dsCustomerInfo.Tables["Customer"].Rows[0]["PromotionCode"].ToString().Trim()))
                            {
                                hdnPromotionalCode.Value = dsCustomerInfo.Tables["Customer"].Rows[0]["PromotionCode"].ToString().Trim();
                            }



                            //Call private method to show the customer info on Left Navigation Bar.

                        }





                        dvNoDataFound.Visible = false;
                        grdCustomerDetail.Visible = true;
                        //btnGo.Visible = false;
                        //txtCardNumber.ReadOnly = true;
                        btnMerge.Visible = true;

                        grdCustomerDetail.DataSource = dsCustomerInfo.Tables["Customer"];
                        grdCustomerDetail.DataBind();
                    }
                    else
                    {
                        //btnGo.Visible = true;
                        //txtCardNumber.ReadOnly = false;
                        btnMerge.Visible = false;
                        dvNoDataFound.Visible = true;
                        grdCustomerDetail.Visible = false;
                    }
                }
                else
                {

                    btnMerge.Visible = false;
                    //dvNoDataFound.Visible = true;
                    grdCustomerDetail.Visible = false;
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC MergeCard.FindCustomer()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC MergeCard.FindCustomer() input Xml-" + conditionXml);
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

        protected bool ValidatePage()
        {
            bool bErrorFlag = true;
            string regNumeric = @"^[0-9]*$";
            txtCardNumber.CssClass = "";
            try
            {
                int MaxClubcardlen = Convert.ToInt32(ConfigurationManager.AppSettings["ClubCardMaxLength"].ToString());
                int MinClubcardlen = Convert.ToInt32(ConfigurationManager.AppSettings["ClubCardMinLength"].ToString());
                //Card number
                if (!Helper.IsRegexMatch(txtCardNumber.Text.Trim(), regNumeric, false, false))
                {
                    //errMsgCardNumber = "Please enter a valid Card Number";
                    lblSuccessMessage.Text = GetLocalResourceObject("ValidCardNo.Text").ToString();
                    txtCardNumber.Text = "";
                    txtCardNumber.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                //Card number should be more between 16  and 18 digits
                else if (!string.IsNullOrEmpty(txtCardNumber.Text.Trim()) && (txtCardNumber.Text.Trim().Length < MinClubcardlen || txtCardNumber.Text.Trim().Length > MaxClubcardlen))
                {
                    // errMsgCardNumber = "Please enter a valid Card Number";
                    lblSuccessMessage.Text = GetLocalResourceObject("ValidCardNo.Text").ToString();
                    txtCardNumber.Text = "";
                    txtCardNumber.CssClass = "errorFld";
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

        protected void GrdCustomerDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    //e.Row.Cells[1].Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
                    //e.Row.Cells[2].Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

                    Literal ltrName = (Literal)e.Row.FindControl("ltrName");


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

        protected void btnMerge_Click(object sender, EventArgs e)
        {

            Hashtable searchData = null;
            string objectXML = string.Empty;
            string resultXml = string.Empty;

            long objectID = 0;

            serviceClient = new ClubcardServiceClient();
            //For pagination



            try
            {
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                {
                    searchData = new Hashtable();
                    // Literal ltrName = (Literal)e.Row.FindControl("ltrName");


                    searchData["ClubcardID"] = hdnClubcard.Value.ToString();
                    searchData["MergeClubcardID"] = txtCardNumber.Text.ToString();
                    //searchData["Culture"] = ConfigurationManager.AppSettings["Culture"].ToString();
                    //Preparing parameters for service call
                    objectXML = Helper.HashTableToXML(searchData, "MergeCards");



                    // customerObj = new CustomerServiceClient();
                    #region Trace Start
                    NGCTrace.NGCTrace.TraceInfo("Start: CSC SearchCustomer.FindCustomer()");
                    NGCTrace.NGCTrace.TraceDebug("Start: CSC SearchCustomer.FindCustomer() input Xml-" + objectXML);
                    #endregion

                    // if (serviceClient.MergeCards(out resultXml, conditionXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                    if (serviceClient.MergeCards(out resultXml, out objectID, objectXML, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                    {


                        if (objectID == 501)
                        {
                            lblSuccessMessage.Text = GetLocalResourceObject("Fivenotone").ToString();
                            btnMerge.Visible = false;
                            grdCustomerDetail.Visible = false;
                            txtCardNumber.Text = "";
                            //"CardNumber is not within a valid range.";
                        }
                        else if (objectID == 502)
                        {
                            lblSuccessMessage.Text = GetLocalResourceObject("Fivenottwo").ToString();
                            btnMerge.Visible = false;
                            grdCustomerDetail.Visible = false;
                            txtCardNumber.Text = "";
                            //"Both the CardNumber is are same.";
                        }
                        else if (objectID == 503)
                        {
                            lblSuccessMessage.Text = GetLocalResourceObject("Fivenotthree").ToString();
                            btnMerge.Visible = false;
                            grdCustomerDetail.Visible = false;
                            txtCardNumber.Text = "";
                            //"Primary clubcard for both Merged and merge with are same";
                        }
                        else
                        {
                            if (objectID != null)
                            {
                                if (Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString()) != objectID)
                                {
                                    if (Helper.GetTripleDESEncryptedCookieValue("CustomerID") != null)
                                    {
                                        Helper.DeleteTripleDESEncryptedCookie("CustomerID");
                                    }

                                    Helper.SetTripleDESEncryptedCookie("CustomerID", objectID.ToString());
                                    txtCardNumber.Text = "";
                                    btnMerge.Visible = false;
                                    grdCustomerDetail.Visible = false;
                                    long customerID = objectID;
                                    string amendBy = string.Empty;
                                    string amendDateTime = string.Empty;
                                    ShowCustomerInfoOnLeftNav(hdnTitle.Value, hdnFname.Value, hdnMname.Value, hdnLname.Value, hdncardNumber.Value, hdnhouseHoldID.Value, hdncurrentPoints.Value, hdnjoinDate.Value, hdnJoinRouteCode.Value, hdnPromotionalCode.Value, customerID, hdnamendBy.Value, hdnamendDateTime.Value);
                                    string Url = "MergeCard.aspx?MergeSuccess=Success";
                                    Response.Redirect(Url, false);

                                }
                            }

                        }

                    }
                    else
                    {
                        throw new Exception("Merge cards failed for condition: " + objectXML);
                    }

                }
                else
                {
                    Response.Redirect("Default.aspx", false);
                }


                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC SearchCustomer.FindCustomer()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC SearchCustomer.FindCustomer() input Xml-" + objectXML);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC SearchCustomer.FindCustomer() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID") + "; Search Condition: " + objectXML);
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

    }




}






