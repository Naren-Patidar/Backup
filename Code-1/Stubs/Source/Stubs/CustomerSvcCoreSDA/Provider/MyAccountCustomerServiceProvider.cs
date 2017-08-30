using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using ServiceUtility;

namespace MyAccountCustomerService
{
    public class MyAccountCustomerServiceProvider
    {

        Helper helper = new Helper();
        #region ServiceMethods

        public long CustomerMartiniIDGet(int dxshCustomerID)
        {
            string resultXml = string.Empty;
            string fileName = string.Empty;
            string xmlFile = string.Empty;
            resultXml = string.Empty;
            XmlDocument input = new XmlDocument();
            long customerID = 0;
            try
            {
                xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\CustomerMartiniIDGet\{0}.xml", dxshCustomerID));                
                resultXml = helper.LoadXMLFile(xmlFile);
                input.LoadXml(resultXml);
                XmlNode custIDNode = input.SelectSingleNode("//MartiniCustomerID");
                if (custIDNode != null)
                {
                    customerID = Convert.ToInt64(custIDNode.InnerText);
                }

            }
            catch (Exception ex)
            {
                resultXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
            }
            return customerID;
        }
      
        public string GetPersonalDetails(long webCustomerID)
        {
            string resultXml = string.Empty;
            string fileName = string.Empty;
            string xmlFile = string.Empty;
            resultXml = string.Empty;
            try
            {
                xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\GetPersonalDetails\{0}.xml", webCustomerID));
                resultXml = helper.LoadXMLFile(xmlFile);
            }
            catch (Exception ex)
            {
                resultXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);                
            }
            return resultXml;
        }

        public string GetHomeAddress(long webCustomerID)
        {
            string resultXml = string.Empty;
            string fileName = string.Empty;
            string xmlFile = string.Empty;
            resultXml = string.Empty;
            try
            {
                xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\GetHomeAddress\{0}.xml", webCustomerID));
                resultXml = helper.LoadXMLFile(xmlFile);
            }
            catch (Exception ex)
            {
                resultXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
            }
            return resultXml;
        }

     
        #endregion
    }
}