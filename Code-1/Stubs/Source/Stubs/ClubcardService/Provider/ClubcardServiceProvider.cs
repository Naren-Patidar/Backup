using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using ServiceUtility;

namespace ClubcardService
{
    public class ClubcardServiceProvider
    {
        Helper helper = new Helper();


        #region Service Methods
        public bool IGHSCheckCustomerActivated(out char activated, out long customerID, out string errorXml, out string resultXml, string dotcomCustomerID, string culture)
        {
            bool chk = true;
            XmlDocument response = new XmlDocument();
            string xmlFile = string.Empty;
            errorXml = string.Empty;
            try
            {
                XmlNode responseNode ;
                xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource/IGHSCheckCustomerActivated/{0}/IGHSCheckCustomerActivated.xml",culture));
                response = helper.LoadXMLDoc(xmlFile);
                // create ns manager
                XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
                xmlnsManager.AddNamespace("def", "http://tesco.com/clubcardonline/datacontract/2010/01");

                XmlNode errorNode = response.SelectSingleNode("//def:errorXml", xmlnsManager);
                errorXml = (errorNode != null) ? errorNode.InnerXml : string.Empty;
                XmlNode resultXmlNode = response.SelectSingleNode("//def:resultXml", xmlnsManager);
                resultXml = (resultXmlNode != null) ? resultXmlNode.InnerXml : string.Empty;
                XmlNode customerIDNode = response.SelectSingleNode("//def:customerID", xmlnsManager);
                customerID = Convert.ToInt64((customerIDNode != null) ? customerIDNode.InnerXml : string.Empty);
                XmlNode activatedNode = response.SelectSingleNode("//def:Activated", xmlnsManager);
                activated = Convert.ToChar((activatedNode != null) ? activatedNode.InnerXml : string.Empty);
                XmlNode resultNode = response.SelectSingleNode("//def:IGHSCheckCustomerActivatedResult", xmlnsManager);
                responseNode = response.SelectSingleNode("//def:Activated", xmlnsManager);
                responseNode.InnerText = "Y";
                responseNode = response.SelectSingleNode("//def:activated", xmlnsManager);
                responseNode.InnerText = "Y";
                responseNode = response.SelectSingleNode("//def:customerID", xmlnsManager);
                responseNode.InnerText = "904289";
                responseNode = response.SelectSingleNode("//def:CustomerID", xmlnsManager);
                responseNode.InnerText = "904289";


                responseNode = response.SelectSingleNode("//def:ViewHouseholdStatusOfCustomer", xmlnsManager);
                if (responseNode == null)
                {
                    XmlNode newDataSet = response.SelectSingleNode("//def:NewDataSet[position() = last()]", xmlnsManager);
                    XmlNode ViewHouseholdStatusOfCustomer = response.CreateNode(XmlNodeType.Element, "ViewHouseholdStatusOfCustomer", "http://tesco.com/clubcardonline/datacontract/2010/01");
                    XmlNode PrimaryCustomerID = response.CreateNode(XmlNodeType.Element, "PrimaryCustomerID", "http://tesco.com/clubcardonline/datacontract/2010/01");
                    PrimaryCustomerID.InnerText = "904289";
                    ViewHouseholdStatusOfCustomer.AppendChild(PrimaryCustomerID);
                    XmlNode CustomerUseStatus = response.CreateNode(XmlNodeType.Element, "CustomerUseStatus", "http://tesco.com/clubcardonline/datacontract/2010/01");
                    CustomerUseStatus.InnerText = "1";
                    ViewHouseholdStatusOfCustomer.AppendChild(CustomerUseStatus);
                    XmlNode CustomerMailStatus = response.CreateNode(XmlNodeType.Element, "CustomerMailStatus", "http://tesco.com/clubcardonline/datacontract/2010/01");
                    CustomerMailStatus.InnerText = "7";
                    ViewHouseholdStatusOfCustomer.AppendChild(CustomerMailStatus);
                    XmlNode CustomerEmailStatus = response.CreateNode(XmlNodeType.Element, "CustomerEmailStatus", "http://tesco.com/clubcardonline/datacontract/2010/01");
                    CustomerEmailStatus.InnerText = "7";
                    ViewHouseholdStatusOfCustomer.AppendChild(CustomerEmailStatus);
                    XmlNode CustomerMobilePhoneStatus = response.CreateNode(XmlNodeType.Element, "CustomerMobilePhoneStatus", "http://tesco.com/clubcardonline/datacontract/2010/01");
                    CustomerMobilePhoneStatus.InnerText = "7";
                    ViewHouseholdStatusOfCustomer.AppendChild(CustomerMobilePhoneStatus);
                    newDataSet.AppendChild(ViewHouseholdStatusOfCustomer);

                    helper.WriteXmlFile(xmlFile, response.InnerXml);
                }
                return Convert.ToBoolean(resultNode.InnerXml);
                
            }
            catch (Exception ex)
            {
                errorXml = string.Format("{0} |Detail: {1}", ex.Message, ex.StackTrace);
                chk = false;
                activated=' ';
                resultXml = "";
                customerID= 0;
                return chk;
            }
            
        }
        public bool GetMyAccountDetails(out string errorXml, out string resultXml, long CustomerID, string culture)
        {
            bool chk = true;
            string fileName = string.Empty;
            errorXml = string.Empty;
            resultXml = string.Empty;
            try
            {
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\GetMyAccountDetails\{0}\{1}.xml", culture, CustomerID));
                resultXml = helper.LoadXMLFile(fileName);
            }
            catch (Exception ex)
            {
                errorXml = string.Format("{0} |Detail: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            return chk;
        }

        public bool GetClubcardsCustomer(out string errorXml, out string resultXml, long CustomerID, string culture)
        {
            bool chk = true;
            errorXml = string.Empty;
            resultXml = string.Empty;
            try
            {
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("DataSource/GetClubcardsCustomer/{0}/{1}.xml", culture, CustomerID));
                resultXml = helper.LoadXMLFile(fileName);                
            }
            catch (Exception ex)
            {
                errorXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            return chk;
        }

        public bool GetHouseholdCustomers(out string errorXml, out string resultXml, long CustomerID, string culture)
        {
            bool chk = true;
            errorXml = string.Empty;
            resultXml = string.Empty;
            try
            {
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("DataSource/GetHouseholdCustomers/{0}/{1}/{2}.xml", culture, CustomerID,CustomerID));
                XmlDocument response = helper.LoadXMLDoc(fileName);
                XmlNode resultNode = response.SelectSingleNode("//resultXml");
                if (resultNode != null)
                {
                    resultXml = resultNode.InnerXml;
                }
                else
                {
                    errorXml = "Data not recorded for this scenario";
                    chk = false;
                }
            }
            catch (Exception ex)
            {
                errorXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            return chk;
        }

        public bool GetTxnDetailsByCustomerAndOfferID(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            bool chk = true;
            errorXml = string.Empty;
            resultXml = string.Empty;
            rowCount = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(conditionXml);
            string offerID = xmlDoc.SelectSingleNode("//OfferID").InnerText;
            string strCustID = xmlDoc.SelectSingleNode("//CustomerID").InnerText;
            try
            {
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("DataSource/GetTxnDetailsByCustomerAndOfferID/{0}/{1}/{2}.xml", culture,strCustID, offerID));
                resultXml = helper.LoadXMLFile(fileName);
            }
            catch (Exception ex)
            {
                errorXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            return chk;
        }

        public bool GetChristmasSaverSummary(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            bool chk = true;
            errorXml = string.Empty;
            resultXml = string.Empty;
            rowCount = 0;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(conditionXml);
                string strCustID = xmlDoc.SelectSingleNode("//CustomerID").InnerText;
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("DataSource/GetChristmasSaverSummary/{0}/{1}/GetChristmasSaverSummary.xml", culture, strCustID));
                resultXml = helper.LoadXMLFile(fileName);
            }
            catch (Exception ex)
            {
                errorXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            return chk;
        }

        public bool GetPointsForAllCollPeriodByCustomer(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            bool chk = true;
            errorXml = string.Empty;
            resultXml = string.Empty;
            rowCount = 0;
            rowCount = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(conditionXml);
            string custID = xmlDoc.SelectSingleNode("//CustomerID").InnerText;

            try
            {
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("DataSource/GetPointsForAllCollPeriodByCustomer/{0}/{1}.xml", culture,custID));
                resultXml = helper.LoadXMLFile(fileName);
            }
            catch (Exception ex)
            {
                errorXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            return chk;
        }


        public bool GetPointsSummaryInfo(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            bool chk = true;
            errorXml = string.Empty;
            resultXml = string.Empty;
            rowCount = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(conditionXml);
            string offerID = xmlDoc.SelectSingleNode("//OfferID").InnerText;
            string custID = xmlDoc.SelectSingleNode("//CustomerID").InnerText;
            try
            {
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("DataSource/GetPointsSummaryInfo/{0}/{1}/{2}.xml", culture,custID,offerID));
                resultXml = helper.LoadXMLFile(fileName);
            }
            catch (Exception ex)
            {
                errorXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            return chk;
        }

        

        public bool IsNewOrderReplacementValid(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            bool chk = true;
            errorXml = string.Empty;
            resultXml = string.Empty;
            rowCount = 0;
            string fileName = string.Empty;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(conditionXml);
                string customerID = xmlDoc.SelectSingleNode("//CustomerID").InnerText;
                List<string> preferenceTypes = conditionXml.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();


                fileName = string.Format(@"DataSource\IsNewOrderReplacementValid\{0}.xml",customerID);
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
             

                XmlDocument response = helper.LoadXMLDoc(fileName);
            
                XmlNode root = response.DocumentElement;

                // create ns manager
                XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
                xmlnsManager.AddNamespace("def", "http://tesco.com/clubcardonline/datacontract/2010/01");

                XmlNode resultNode = response.SelectSingleNode("//def:resultXml", xmlnsManager);
                resultXml = (resultNode != null) ? resultNode.InnerXml : string.Empty;

                XmlNode errorNode = response.SelectSingleNode("//def:errorXml", xmlnsManager);
                errorXml = (errorNode != null) ? errorNode.InnerXml : string.Empty;

                XmlNode rowCountNode = response.SelectSingleNode("//def:rowCount", xmlnsManager);
                rowCount = (Convert.ToInt32((rowCountNode != null) ? rowCountNode.InnerXml : string.Empty));


            }
            catch (Exception ex)
            {
                errorXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            return chk;
        }


        public bool IsXmasClubMember(out string errorXml, out string resultXml, long CustomerID, string culture)
        {
            bool chk = true;
            errorXml = string.Empty;
            resultXml = string.Empty;
           

            try
            {
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("DataSource/IsXmasClubMember/{0}/{1}.xml", culture, CustomerID));
                resultXml = helper.LoadXMLFile(fileName);
            }
            catch (Exception ex)
            {
                errorXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            return chk;
        }

        public bool AddNewOrderReplacement(out string errorXml, out long customerID, string updateXml, string consumer)
        {
            bool chk = true;
            errorXml = string.Empty;
            string fileName = string.Empty;
            XmlDocument response = new XmlDocument();
            XmlNode responseNode;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(updateXml);
                string custID = xmlDoc.SelectSingleNode("//CustomerID").InnerText;
               
                fileName = string.Format(@"DataSource\IsNewOrderReplacementValid\{0}.xml", custID);
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                if (File.Exists(fileName))
                {
                    response = helper.LoadXMLDoc(fileName);
                }
                // create ns manager
                XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
                xmlnsManager.AddNamespace("def", "http://tesco.com/clubcardonline/datacontract/2010/01");
                string reasonCode = xmlDoc.SelectSingleNode("//RequestReasonCode").InnerText;
                    responseNode = response.SelectSingleNode("//def:noOfDaysLeftToProcess", xmlnsManager);
                    responseNode.InnerText = Convert.ToString(0) ;
                    responseNode = response.SelectSingleNode("//def:standardCardNumber", xmlnsManager);
                    responseNode.InnerText = "";
                    responseNode = response.SelectSingleNode("//def:ClubcardTypeIndicatior", xmlnsManager);
                    responseNode.InnerText = "N";
                    responseNode = response.SelectSingleNode("//def:oldOrderExists", xmlnsManager);
                    responseNode.InnerText = Convert.ToString(1);
                    helper.WriteXmlFile(fileName, response.InnerXml);
            }
            catch (Exception ex)
            {
                errorXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            customerID = -1;
            return chk;
        }

        public bool ResetOrderReplacementData(out string errorXml,  long customerID,string ClubcardNumber,string Culture)
        {
            bool chk = true;
            errorXml = string.Empty;
            string fileName = string.Empty;
            XmlDocument response = new XmlDocument();
            XmlNode responseNode;

            try
            {
                fileName = string.Format(@"DataSource\IsNewOrderReplacementValid\{0}.xml", customerID);
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                if (File.Exists(fileName))
                {
                    response = helper.LoadXMLDoc(fileName);
                }
               
                XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
                xmlnsManager.AddNamespace("def", "http://tesco.com/clubcardonline/datacontract/2010/01");
                responseNode = response.SelectSingleNode("//def:noOfDaysLeftToProcess", xmlnsManager);
                responseNode.InnerText = Convert.ToString(14);
                responseNode = response.SelectSingleNode("//def:standardCardNumber", xmlnsManager);
                responseNode.InnerText = Convert.ToString(ClubcardNumber);
                responseNode = response.SelectSingleNode("//def:ClubcardTypeIndicatior", xmlnsManager);
                responseNode.InnerText = "";
                responseNode = response.SelectSingleNode("//def:oldOrderExists", xmlnsManager);
                responseNode.InnerText = Convert.ToString(0);
                helper.WriteXmlFile(fileName, response.InnerXml);
            }
            catch (Exception ex)
            {
                errorXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            customerID = -1;
            return chk;
        }
        #endregion
    }
}
