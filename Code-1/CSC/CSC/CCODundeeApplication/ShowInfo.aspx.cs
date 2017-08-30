using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using CCODundeeApplication.CustomerService;
using System.ServiceModel;
using System.Globalization;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CCODundeeApplication
{
    public partial class ShowInfo : System.Web.UI.Page
    {
        #region Local varibales

        Hashtable htCustomer = null;
        CustomerServiceClient customerClient = null;
        DataSet dsResult = null;
        string culture = string.Empty;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            GetTransactionDetails();
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
        public void GetTransactionDetails()
        {
            try
            {
             

                customerClient = new CustomerServiceClient();
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                int rowCount = 0;
                int maxCount = 0;
                XmlDocument resulDoc = null;
                htCustomer = new Hashtable();
                htCustomer["OfferNumberToShow"] = Request.QueryString[0].ToString().Split(',')[1];
                htCustomer["PrimaryCustomerID"] = 0;
                htCustomer["CustomerID"] = Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString();
                htCustomer["ClubcardTransactionID"] = Request.QueryString[0].ToString().Split(',')[0];
                htCustomer["HouseHold"] = "Y";
                string objectXml = Helper.HashTableToXML(htCustomer, "customer");

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ShowInfo.GetTransactionDetails()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ShowInfo.GetTransactionDetails() Input Xml-" + objectXml);
                #endregion

                if (customerClient.TransactionsByOffer(out errorXml, out resultXml, out rowCount, objectXml,maxCount,"en-GB"))
                {
                    
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsResult = new DataSet();
                    dsResult.ReadXml(new XmlNodeReader(resulDoc));

                    FillValues(dsResult);
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC ShowInfo.GetTransactionDetails()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC ShowInfo.GetTransactionDetails() Input Xml-" + objectXml);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ShowInfo.GetTransactionDetails() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC ShowInfo.GetTransactionDetails() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ShowInfo.GetTransactionDetails()");
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

        public void FillValues(DataSet ds)
        {
            string dateFormat = ConfigurationManager.AppSettings["DateDisplayFormat"];
            DateTime dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["TransactionDateTime"].ToString());
            string culture = ConfigurationSettings.AppSettings["Culture"].ToString();
            modalCardNumber.InnerText = ds.Tables[0].Rows[0]["ClubcardID"].ToString();
            modalTotalPoints.InnerText = ds.Tables[0].Rows[0]["TotalPoints"].ToString();
            modalDateofTransaction.InnerText = dt.ToString(dateFormat);
            modalPartnerPoints.InnerText = ds.Tables[0].Rows[0]["GreenPointsQty"].ToString();
            modalPoints.InnerText = ds.Tables[0].Rows[0]["NormalPoints"].ToString();
            modalPosNumber.InnerText = ds.Tables[0].Rows[0]["SourcePOSID"].ToString();
            modalProductPoints.InnerText = ds.Tables[0].Rows[0]["SKUPointsQty"].ToString();
            modalTransactionType.InnerText = ds.Tables[0].Rows[0]["TransactionTypeDecription"].ToString();
            modalTransReason.InnerText = ds.Tables[0].Rows[0]["TransactionReasonDecription"].ToString();
            modalTxnSTPFPoints.InnerText = ds.Tables[0].Rows[0]["ManualPointsQty"].ToString();
            modalWelcomePoints.InnerText = ds.Tables[0].Rows[0]["WelcomePointsQty"].ToString();
            modalAmountSpent.InnerText = ds.Tables[0].Rows[0]["AmountSpent"].ToString();
            modalReceiptNumber.InnerText = ds.Tables[0].Rows[0]["RecieptNumber"].ToString();
            modalStoreorPartner.InnerText = ds.Tables[0].Rows[0]["TescoStoreOrPartnerID"].ToString();
            
        }
    }
}
