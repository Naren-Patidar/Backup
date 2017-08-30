using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using CCODundeeApplication.CustomerService;
using System.ServiceModel;
using System.Xml.Linq;
using System.Net;
using System.Net.Sockets;

namespace CCODundeeApplication
{
    public partial class Ajax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static object GetAccountStatus()
        {
            object status = null;
            CustomerServiceClient customerObj = new CustomerServiceClient();
            try
            {
                string errorXml = string.Empty, resultXml = string.Empty, customerID = Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString();
                if (customerObj.GetCustomerVerificationDetails(out errorXml, out resultXml, customerID))
                {
                    XDocument resultDoc = new XDocument();
                    resultDoc = XDocument.Parse(resultXml);
                    var configDictionary =  (from configDatum in resultDoc.Descendants("SecurityStatus").First().Descendants()
                                          select new {
                                            Name = configDatum.Name.LocalName,
                                            Value = configDatum.Value,
                                          }).ToDictionary(o => o.Name, o => o.Value);
                    status = configDictionary;
                }
                else
                {
                    throw new Exception(errorXml);
                }
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
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

        [WebMethod]
        public static object UnlockAccount()
        {
            object status = true;
            CustomerServiceClient customerObj = new CustomerServiceClient();
            try
            {
                string errorXml = string.Empty, resultXml = string.Empty, customerID = Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString();
                string ipAddress = getIPAddress();
                long lngCustomerID;
                string condXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-16\"?><CustomerVerification><Browserused>IE</Browserused><CustomerID>{0}</CustomerID><IsValidAttempt>Y</IsValidAttempt><IPAddress>{1}</IPAddress></CustomerVerification>" , customerID, ipAddress);
                if (!customerObj.InsertUpdateCustomerVerificationDetails(out lngCustomerID, out resultXml, condXml))
                {
                    throw new Exception(errorXml);
                }
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
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

        static string getIPAddress()
        {
            string ipAddress = string.Empty;
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch { ipAddress = "127.0.0.1"; }
            return ipAddress;
        }
    }
}