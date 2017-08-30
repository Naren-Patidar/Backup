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
using CCODundeeApplication.ClubcardService;
using CCODundeeApplication.CustomerService;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;

namespace CCODundeeApplication
{
    public partial class ChangePrimaryCard : System.Web.UI.Page
    {
      
        ClubcardServiceClient serviceClient=null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string CustomerID = Request.QueryString["custID"].ToString().Replace(" ", "+");
                  CustomerID=  CryptoUtil.DecryptTripleDES(CustomerID);
                GetClubCards(long.Parse(CustomerID));
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


        public void GetClubCards(long CustomerID)
        {

            try
            {
                serviceClient=new ClubcardServiceClient();
                string resultXml, errorXml;
                string culture=ConfigurationManager.AppSettings["Culture"].ToString();
                DataView dvFilter = new DataView();

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardType.GetClubCards()  CustomerID-" + CustomerID);
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardType.GetClubCards() CustomerID-" + CustomerID);
                #endregion

                if (serviceClient.GetClubcardsCustomer(out errorXml, out resultXml, CustomerID, culture))
                    {
                        if (resultXml != "" && resultXml != "<NewDataSet />")
                        {
                            XmlDocument resulDoc = new XmlDocument();
                            resulDoc.LoadXml(resultXml);
                            DataSet dsClubcard = new DataSet();
                            dsClubcard.ReadXml(new XmlNodeReader(resulDoc));

                            ////Bind Dropdown List with Clubcards

                            if (dsClubcard.Tables["ClubcardDetails"] != null)
                            {
                                dvFilter = dsClubcard.Tables["ClubcardDetails"].DefaultView;
                                //For UK
                                if (culture.Equals("en-GB"))
                                {
                                    dvFilter.RowFilter = "ClubcardStatusDescEnglish='Normal'";
                                }
                                //For Group Countries
                                else
                                {
                                    //StatusInChangePrimaryScreen
                                    dvFilter.RowFilter = "ClubcardStatusDescEnglish='" + ConfigurationManager.AppSettings["StatusInChangePrimaryScreen"].ToString() + "'";

                                }                                
                                FillDropDownList(dvFilter);
                            }
                        }
                    }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CardType.GetClubCards() CustomerID-" + CustomerID);
                NGCTrace.NGCTrace.TraceDebug("End: CSC CardType.GetClubCards() CustomerID-" + CustomerID);
                #endregion
                
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardRanges.GetClubCards()  CustomerID-" + CustomerID + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardRanges.GetClubCards()  CustomerID-" + CustomerID + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardRanges.GetClubCards()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
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

        #region Fill Groups Drop Down

        public void FillDropDownList(DataView dvClubcards)
        {

            ddlClubcards.DataMember = "ClubcardID";
            ddlClubcards.DataTextField = "ClubcardID";
            ddlClubcards.DataValueField = "ClubcardID";
            ddlClubcards.DataSource = dvClubcards;
            ddlClubcards.DataBind();
        }
        #endregion


        protected void imgConfirm_Click(object sender, ImageClickEventArgs e)
        {
            string clubCardID = ddlClubcards.SelectedValue.ToString();
            if(UpdatePrimaryCard(clubCardID))
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "refreshParent()", true);
            }
            else
            {

            }
        }


        #region ChangePrimaryCard
        /// <summary>
        /// Update the card as Primary
        /// </summary>
        /// <param name="clubcardID"></param>
        /// <returns></returns>
        private bool UpdatePrimaryCard(string clubcardID)
        {
            bool success = true;
            CustomerServiceClient customerserviceclient = new CustomerServiceClient();
            string insertXml = string.Empty;
            try
            {
                Hashtable htCardNo = new Hashtable();
                string ClubcardID = clubcardID;
                htCardNo["ClubcardID"] = ClubcardID;

                string CustomerID = Request.QueryString["custID"].ToString().Replace(" ", "+");
                CustomerID = CryptoUtil.DecryptTripleDES(CustomerID);
                htCardNo["custid"] = long.Parse(CustomerID);
                insertXml = CCODundeeApplication.Helper.HashTableToXML(htCardNo, "PhoneNumber");
                string resultXml = string.Empty;
                string errorXml = string.Empty;

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardType.GetClubCards() CustomerID-" + CustomerID);
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardType.GetClubCards() CustomerID-" + CustomerID + "Input Xml-" + insertXml);
                #endregion
                
                if (customerserviceclient.UpdatePrimaryClubcardID(out errorXml, insertXml))
                {
                    success = true;
                }
                else
                {
                    success = false;
                }

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardType.GetClubCards() CustomerID-" + CustomerID);
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardType.GetClubCards() CustomerID-" + CustomerID + "Input Xml-" + insertXml);
                #endregion

            }
            catch (Exception ex)
            {
                success = false;
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardRanges.GetClubCards() Input Xml-" + insertXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardRanges.GetClubCards() - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardRanges.GetClubCards()");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                #endregion Trace Error
                throw ex;
            }
            return success;
        }
        #endregion
    }
}
