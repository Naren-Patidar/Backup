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
using CCODundeeApplication.ClubcardService;
using System.Configuration;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;

namespace CCODundeeApplication
{
    public partial class ShowCallDetails : System.Web.UI.Page
    {
        #region Private Variables

        protected ClubcardServiceClient serviceClient = null;
        string culture = string.Empty;
        DataSet dsCustomerInfo = null;
        string RowNumber = string.Empty;

        #endregion Private Variables

        /// <summary>
        /// Get all the call logged Details from Database
        /// Hold the Query String Value 
        /// </summary>

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    RowNumber = Request.QueryString["arg"].ToString();
                    GetCallDetails();
                }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical:ShowCallDetails.Page_Load - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ShowCallDetails.Page_Load - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ShowCallDetails.Page_Load");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        /// <summary>
        /// Fetch All The Call logged Details
        /// </summary>

        private void GetCallDetails()
        {
            string resultXml = string.Empty;
            XmlDocument resulDoc = null;
            try
            {
                long CustomerId = Convert.ToUInt32(Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                string ClutId = ConfigurationManager.AppSettings["Culture"].ToString();
                serviceClient = new ClubcardServiceClient();
                if (serviceClient.ViewCustomerHelplineInformation(out resultXml, CustomerId, ClutId))
                {
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsCustomerInfo = new DataSet();
                    dsCustomerInfo.ReadXml(new XmlNodeReader(resulDoc));

                    //Save the dataset to view state for postback cycles
                    ViewState["dsCallDetails"] = dsCustomerInfo;

                    if (dsCustomerInfo.Tables.Count > 0)
                    {
                        FillValues(dsCustomerInfo);
                    }
                    else
                    {
                    lblCallDetails.Text = GetLocalResourceObject("lblCallDetails").ToString();
                    }
                }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical:ShowCallDetails.GetCallDetails - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ShowCallDetails.GetCallDetails - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ShowCallDetails.GetCallDetails");
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
        /// Fetch Call Details based on request
        /// Show Call details for a customer.
        /// </summary>

        public void FillValues(DataSet ds)
        {
            try
            {
                DataTable dt = new DataTable();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string RowDate = ds.Tables[0].Rows[i][0].ToString();

                    modalDateLogged.InnerText = "";
                    modalLoggedBy.InnerText = "";
                    txtCallDteails.Text = "";
                    modalReason.InnerText = "";

                    if (RowDate == RowNumber)
                    {
                        modalDateLogged.InnerText = ds.Tables[0].Rows[i]["DateLogged"].ToString();
                        modalLoggedBy.InnerText = ds.Tables[0].Rows[i]["LoggedBy"].ToString();
                        txtCallDteails.Text = ds.Tables[0].Rows[i]["FullCallDetail"].ToString();
                        modalReason.InnerText = ds.Tables[0].Rows[i]["ReasonCodeID"].ToString();
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical:ShowCallDetails.FillValues - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ShowCallDetails.FillValues - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ShowCallDetails.FillValues");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }

        }
    }
}