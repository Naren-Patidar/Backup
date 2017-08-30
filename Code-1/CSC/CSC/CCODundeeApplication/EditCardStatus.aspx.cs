using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using CCODundeeApplication.CustomerService;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;
using System.Configuration;
using System.Data;
using System.Collections;

namespace CCODundeeApplication
{
    public partial class EditCardStatus : System.Web.UI.Page
    {

        #region Private Variables

        Hashtable htCustomer = null;

        CustomerServiceClient customerClient = null;

        string culture = null;

        DataSet dsStatus = null;

        #endregion Private Variables

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            AttachClientScripts();
            if (!IsPostBack)
            {
                //Commented by Noushad
                //imgConfirm.Attributes.Add("onclick", "javascript:return " + "confirm('Are you sure you would like to block your card?')");
                GetCardStatus();
            }
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
                Response.Redirect("~/Default.aspx", false);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Method to register the ClientID of controls.
        /// </summary>
        private void AttachClientScripts()
        {
            //Getting and Setting CouponsCanBeSelectedFromFirstRow to variable.
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "RegisterDdlStatusClientID", "ddlStatusClientId = '" + ddlStatus.ClientID + "';", true);
        }

        private void GetCardStatus()
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
                dsStatus = new DataSet();
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC EditCardStatus.GetCardStatus()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC EditCardStatus.GetCardStatus()");
                #endregion
                if (customerClient.GetCardStatus(out errorXml, out resultXml))
                {
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsStatus.ReadXml(new XmlNodeReader(resulDoc));
                    FillDropDownList(dsStatus);
                }
                else
                {
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC EditCardStatus.GetCardStatus()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC EditCardStatus.GetCardStatus()");
                #endregion
            }

            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC EditCardStatus.GetCardStatus() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC EditCardStatus.GetCardStatus() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC EditCardStatus.GetCardStatus()");
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

        private void FillDropDownList(DataSet ds)
        {

            ddlStatus.DataMember = "ClubcardStatusDescEnglish";
            ddlStatus.DataTextField = "ClubcardStatusDescEnglish";
            ddlStatus.DataValueField = "ClubcardStatusID";
            ddlStatus.DataSource = ds.Tables[0];
            ddlStatus.DataBind();
        }

        #endregion Private Methods

        #region UpdateCardStatus
        /// <summary>
        /// Update the card status as Lost/Stolen/Damaged
        /// </summary>
        /// <param name="clubcardID"></param>
        /// <returns></returns>
        private bool UpdateClubcardStatus(Int64 clubcardID)
        {
            CustomerServiceClient customerserviceclient = new CustomerServiceClient();
            bool success = true;
            try
            {
                Hashtable htCardNo = new Hashtable();
                htCardNo["clubcardid"] = clubcardID.ToString();
                htCardNo["custid"] = Helper.GetTripleDESEncryptedCookieValue("CustomerID");
                htCardNo["CardStatus"] = "6";
                string insertXml = CCODundeeApplication.Helper.HashTableToXML(htCardNo, "cardno");
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                string returnMsg = string.Empty;

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC EditCardStatus.UpdateClubcardStatus()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC EditCardStatus.UpdateClubcardStatus() Input Xml-" + insertXml);
                #endregion
                if (customerserviceclient.UpdateCardStatus(out returnMsg, out errorXml, insertXml))
                {
                    success = true;
                }
                else
                {
                    success = false;
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC EditCardStatus.UpdateClubcardStatus()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC EditCardStatus.UpdateClubcardStatus() Input Xml-" + insertXml);
                #endregion
            }
            catch (Exception ex)
            {
                success = false;
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC EditCardStatus.UpdateClubcardStatus() - Error Message :" + ex.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC EditCardStatus.UpdateClubcardStatus() - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC EditCardStatus.UpdateClubcardStatus()");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                #endregion Trace Error
                throw ex;
            }
            return success;
        }
        #endregion
        public void UpdateCardStatusNormal()
        {
            #region Comment Code
            try
            {
                customerClient = new CustomerServiceClient();
                dsStatus = new DataSet();
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                htCustomer = new Hashtable();
                //htCustomer["clubcardid"] = Request.QueryString[0].ToString();
                htCustomer["clubcardid"] = CryptoUtil.DecryptTripleDES(Request.QueryString[0].ToString().Split(',')[0].Replace(" ", "+"));
                htCustomer["custid"] = Request.QueryString[0].ToString().Split(',')[1];//Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString();
                htCustomer["CardStatus"] = int.Parse(ddlStatus.SelectedValue.ToString());
                XmlDocument resulDoc = null;
                string objectxml = Helper.HashTableToXML(htCustomer, "cardno");

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC EditCardStatus.UpdateCardStatusNormal() ");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC EditCardStatus.UpdateCardStatusNormal() Input Xml :" + objectxml);
                #endregion 

                if (customerClient.Test(out errorXml, objectxml))
                {
                    
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "refreshParent('" + ddlStatus.SelectedItem.Text.ToUpper() + "')", true);
                

                }
                else
                {
                    
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC EditCardStatus.UpdateCardStatusNormal() ");
                NGCTrace.NGCTrace.TraceDebug("End: CSC EditCardStatus.UpdateCardStatusNormal() Input Xml :" + objectxml);
                #endregion 
            }

            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC EditCardStatus.UpdateCardStatusNormal() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC EditCardStatus.UpdateCardStatusNormal() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC EditCardStatus.UpdateCardStatusNormal()");
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
            #endregion CommentCode
        }
        #region Event Handlers

        protected void imgConfirm_Click(object sender, ImageClickEventArgs e)
        {
            int CardStatus= int.Parse(ddlStatus.SelectedValue.ToString());

            if (CardStatus == 6)
            {
                if (UpdateClubcardStatus(long.Parse(CryptoUtil.DecryptTripleDES(Request.QueryString[0].ToString().Replace(" ", "+")))))
                {
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "refreshParent('" + ddlStatus.SelectedItem.Text.ToUpper() + "')", true);
                    //GetLocalResourceObject("UniqueConsMsg1.Text").ToString()
                }
                else
                {
                    
                }
            }
            else
            {
                UpdateCardStatusNormal();
            }


            
        }

        #endregion Event Handlers

    }
}
