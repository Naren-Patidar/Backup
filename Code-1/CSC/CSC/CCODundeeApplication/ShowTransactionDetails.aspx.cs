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
    public partial class ShowTransactionDetails : System.Web.UI.Page
    {
        Hashtable htCustomer = null;
        CustomerServiceClient customerClient = null;
        string culture = null;
        DataSet dsStatus = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                imgConfirm.Attributes.Add("onclick", "javascript:return " + "confirm('Are you sure you would like to block your card?')");
                GetCardStatus();

            }
        }


        public void GetCardStatus()
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
            }

            catch (Exception exp)
            {
                Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                    "Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));

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


        #region Fill Groups Drop Down

        public void FillDropDownList(DataSet ds)
        {

            ddlStatus.DataMember = "ClubcardStatusDescEnglish";
            ddlStatus.DataTextField = "ClubcardStatusDescEnglish";
            ddlStatus.DataValueField = "ClubcardStatusID";
            ddlStatus.DataSource = ds.Tables[0];
            ddlStatus.DataBind();
        }
        #endregion

        protected void imgConfirm_Click(object sender, ImageClickEventArgs e)
        {
            
            try
            {
                customerClient = new CustomerServiceClient();
                dsStatus = new DataSet();
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                htCustomer = new Hashtable();
                htCustomer["clubcardid"] = Request.QueryString[0].ToString();
                htCustomer["custid"] = Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString();
                XmlDocument resulDoc = null;
                string objectxml = Helper.HashTableToXML(htCustomer, "cardno");
                if (customerClient.UpdateCardStatus(out errorXml, objectxml,int.Parse(ddlStatus.SelectedValue.ToString())))
                {
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "refreshParent()", true);
                    

                    //FD code has to be writen here
                }
                else
                {
                }
            }

            catch (Exception exp)
            {
                Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                    "Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));

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
    }
}
