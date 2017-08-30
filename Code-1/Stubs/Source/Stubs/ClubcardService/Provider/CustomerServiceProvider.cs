using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using ServiceUtility;
using System.Xml.Linq;
using System.Globalization;
using System.Net;

namespace CustomerService
{
    public class CustomerServiceProvider
    {
        Helper utility = new Helper();
        static string customerID = string.Empty;
        public bool AddPrintAtHomeDetails(out string errorXml, System.Data.DataSet updateDS)
        {
            bool chk = true;
            XmlDocument response = new XmlDocument();
            string xmlFile = string.Empty;
            errorXml = string.Empty;
            try
            {
                xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource/AddPrintAtHomeDetails/AddPrintAtHomeDetails.xml"));
                response = utility.LoadXMLDoc(xmlFile);
                XmlNode root = response.DocumentElement;

                // create ns manager
                XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
                xmlnsManager.AddNamespace("def", "http://tesco.com/clubcardonline/datacontract/2010/01");



                XmlNode errorNode = response.SelectSingleNode("//def:errorXml", xmlnsManager);
                errorXml = (errorNode != null) ? errorNode.InnerXml : string.Empty;
                XmlNode resultNode = response.SelectSingleNode("//def:AddPrintAtHomeDetailsResult", xmlnsManager);
                return Convert.ToBoolean(resultNode.InnerXml);
            }
            catch (Exception ex)
            {
                errorXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            return chk;
        }
        public bool GetCustomerVerificationDetails(out string errorXml, out string resultXml, string conditionXml)
        {
            bool chk = true;
            string xmlFile = string.Empty;
            errorXml = string.Empty;
            resultXml = string.Empty;
            try
            {
                xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource/GetCustomerVerificationDetails/{0}.xml", conditionXml));
                resultXml = utility.LoadXMLFile(xmlFile);
            }
            catch (Exception ex)
            {
                errorXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            return chk;
        }

        public bool InsertUpdateCustomerVerificationDetails(out long customerID, out string resultXml, string updateXml)
        {
            bool chk = true;
            string xmlFile = string.Empty;
            customerID = 0;
            resultXml = string.Empty;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(updateXml);
                if (doc != null)
                {
                    XmlNode custID = doc.SelectSingleNode("//CustomerID");
                    if (custID != null)
                    {
                        string strCustID = custID.InnerText;
                        Int64.TryParse(strCustID, out customerID);
                    }
                }
            }
            catch (Exception ex)
            {
                resultXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            return chk;
        }

        public bool SearchCustomer(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            bool chk = true;
            string xmlFile = string.Empty;
            rowCount = 0;
            resultXml = string.Empty;
            errorXml = string.Empty;
            string strCustID = string.Empty;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(conditionXml);
                if (doc != null)
                {
                    XmlNode custID = doc.SelectSingleNode("//CustomerID");
                    if (custID != null)
                    {
                        strCustID = custID.InnerText;
                    }
                    xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource/SearchCustomer/{0}/{1}/{2}.xml", culture, strCustID, strCustID));
                    XmlDocument response = utility.LoadXMLDoc(xmlFile);

                    XmlNode root = response.DocumentElement;

                    // create ns manager
                    XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
                    xmlnsManager.AddNamespace("def", "http://tesco.com/clubcardonline/datacontract/2010/01");


                    XmlNode resultNode = response.SelectSingleNode("//def:resultXml", xmlnsManager);
                    resultXml = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                    resultXml = resultXml.Replace("xmlns=\"http://tesco.com/clubcardonline/datacontract/2010/01\"", "");

                    XmlNode errorNode = response.SelectSingleNode("//def:errorXml", xmlnsManager);
                    errorXml = (errorNode != null) ? errorNode.InnerXml : string.Empty;
                    XmlNode rowCountNode = response.SelectSingleNode("//def:rowCount", xmlnsManager);
                    rowCount = (rowCountNode != null) ? Int32.TryParse(rowCountNode.InnerXml, out rowCount) ? rowCount : 0 : -1;
                }
            }
            catch (Exception ex)
            {
                resultXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            return chk;
        }

        public bool GetCustomerDetails(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            errorXml = string.Empty;
            rowCount = 0;
            resultXml = string.Empty;
            XmlDocument response = new XmlDocument();

            string clubcard = "a";
            string fileName = string.Empty;
            XmlDocument input = new XmlDocument();
            input.LoadXml(conditionXml);

            // if search by clubcard number
            XmlNode clubcardNode = input.SelectSingleNode("//cardAccountNumber");
            if (clubcardNode != null)
            {
                clubcard = clubcardNode.InnerText;
                fileName = string.Format(@"DataSource\GetCustomerDetails\{0}\{1}.xml", culture, clubcard);
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            }

            //if search by customer id
            XmlNode customerIDNode = input.SelectSingleNode("//CustomerID");
            if (customerIDNode != null)
            {
                string strCustID = customerIDNode.InnerText;
                fileName = string.Format(@"DataSource\GetCustomerDetails\{0}\{1}.xml", culture, strCustID);
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            }

            if (File.Exists(fileName))
            {
                response = utility.LoadXMLDoc(fileName);
            }

            XmlNode root = response.DocumentElement;

            // create ns manager
            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
            xmlnsManager.AddNamespace("def", "http://tesco.com/clubcardonline/datacontract/2010/01");


            XmlNode resultNode = response.SelectSingleNode("//def:resultXml", xmlnsManager);
            resultXml = (resultNode != null) ? resultNode.InnerXml : string.Empty;
            resultXml = resultXml.Replace("xmlns=\"http://tesco.com/clubcardonline/datacontract/2010/01\"", "");
            XmlNode errorNode = response.SelectSingleNode("//def:errorXml", xmlnsManager);
            errorXml = (errorNode != null) ? errorNode.InnerXml : string.Empty;

            XmlNode rowCountNode = response.SelectSingleNode("//def:rowCount", xmlnsManager);
            rowCount = (rowCountNode != null) ? Int32.TryParse(rowCountNode.InnerXml, out rowCount) ? rowCount : 0 : -1;
            return true;
        }


        public bool GetConfigDetails(out string errorXml, out string resultXml, out int rowCount, string conditionXml, string culture)
        {
            bool chk = true;
            string fileName = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            resultXml = string.Empty;

            try
            {

                List<string> preferenceTypes = conditionXml.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                XmlDocument response = null;
                conditionXml = conditionXml.TrimEnd(',');

                fileName = string.Format(@"DataSource\GetConfigDetails\{0}\{1}.xml", culture, conditionXml);
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                response = utility.LoadXMLDoc(fileName);


                // create ns manager
                XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
                xmlnsManager.AddNamespace("def", "http://tesco.com/clubcardonline/datacontract/2010/01");


                string query = string.Empty;
                preferenceTypes.ForEach(pt => query += string.Format("def:ConfigurationType={0} or ", pt));
                query = query.Substring(0, query.Length - 4);
                query = string.Format("not({0})", query);

                XmlNodeList results = response.SelectNodes("//def:NewDataSet/def:ActiveDateRangeConfig[" + query + "]", xmlnsManager);

                XmlNode parent = response.SelectSingleNode("//def:NewDataSet", xmlnsManager);
                foreach (XmlNode node in results)
                {
                    parent.RemoveChild(node);
                }
                string result = parent.OuterXml;
                resultXml = result.Replace("xmlns=\"http://tesco.com/clubcardonline/datacontract/2010/01\"", "");

                XmlNode rowCounts = response.SelectSingleNode("//def:rowCount", xmlnsManager);
                rowCount = Convert.ToInt32(rowCounts.InnerText);


            }
            catch (Exception ex)
            {
                chk = false;
                errorXml = string.Format("{0} | {1}", ex.Message, ex.StackTrace);
            }
            return chk;

        }

        public bool UpdateCustomerDetails(out string errorXml, out long customerID, string updateXml, string consumer)
        {

            XmlDocument response = new XmlDocument();
            string strCustID = string.Empty;
            string strCulture = string.Empty;

            string CustfileName = string.Empty;
            string ClubfileName = string.Empty;

            XmlDocument input = new XmlDocument();
            input.LoadXml(updateXml);


            //if search by customer id
            XmlNode customerIDNode = input.SelectSingleNode("//CustomerID");
            XmlNode culture = input.SelectSingleNode("//Culture");
            if (customerIDNode != null)
            {
                strCustID = customerIDNode.InnerText;
                strCulture = culture.InnerText;
                CustfileName = string.Format(@"DataSource\GetCustomerDetails\{0}\{1}.xml", strCulture, strCustID);
                CustfileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CustfileName);
            }

            var custData = utility.SearchByCustomer(strCustID, AppDomain.CurrentDomain.BaseDirectory);
            string clubcardId = custData.Item2;
            if (File.Exists(CustfileName))
            {
                response = utility.LoadXMLDoc(CustfileName);
            }

            XmlNode root = response.DocumentElement;

            // create ns manager
            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
            xmlnsManager.AddNamespace("def", "http://tesco.com/clubcardonline/datacontract/2010/01");
            XmlNode resultNode;
            XmlNode responseNode;




            #region TitleEnglish
            resultNode = input.SelectSingleNode("//TitleEnglish");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:TitleEnglish", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlTitleEnglish = response.CreateNode(XmlNodeType.Element, "TitleEnglish", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlTitleEnglish.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlTitleEnglish);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:TitleEnglish", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }

            }
            #endregion TitleEnglish


            #region Name1
            string name = null;
            resultNode = input.SelectSingleNode("//Name1");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:Name1", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlName1 = response.CreateNode(XmlNodeType.Element, "Name1", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlName1.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlName1);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:Name1", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }

            }
            #endregion Name1

            #region Name2
            resultNode = input.SelectSingleNode("//Name2");

            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:Name2", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlName2 = response.CreateNode(XmlNodeType.Element, "Name2", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlName2.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlName2);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:Name2", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }

            }
            #endregion Name2

            #region CompleteName
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:CompleteName", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlCompleteName = response.CreateNode(XmlNodeType.Element, "CompleteName", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlCompleteName.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlCompleteName);
                    }
                }
                else
                {
                    responseNode.InnerText = (name != null) ? name : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:CompleteName", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }

            }

            #endregion CompleteName

            #region Name3
            resultNode = input.SelectSingleNode("//Name3");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:Name3", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlName3 = response.CreateNode(XmlNodeType.Element, "Name3", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlName3.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlName3);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:Name3", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }

            }
            #endregion Name3


            #region DateOfBirth
            resultNode = input.SelectSingleNode("//DateOfBirth");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:family_member_1_dob", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlDOB = response.CreateNode(XmlNodeType.Element, "family_member_1_dob", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlDOB.InnerText = (resultNode != null) ? (Convert.ToDateTime(resultNode.InnerXml).ToString("yyyy-MM-ddTHH:mm:ss")).ToString() : string.Empty;
                        customerTag.AppendChild(xmlDOB);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? (Convert.ToDateTime(resultNode.InnerXml).ToString("yyyy-MM-ddTHH:mm:ss")).ToString() : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:family_member_1_dob", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }

            }
            #endregion DateOfBirth


            #region Sex
            resultNode = input.SelectSingleNode("//Sex");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:Sex", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlSex = response.CreateNode(XmlNodeType.Element, "Sex", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlSex.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlSex);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:Sex", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }

            }
            #endregion Sex

            #region MailingAddressLine1
            resultNode = input.SelectSingleNode("//MailingAddressLine1");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:MailingAddressLine1", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlTitleEnglish = response.CreateNode(XmlNodeType.Element, "MailingAddressLine1", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlTitleEnglish.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlTitleEnglish);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:MailingAddressLine1", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }

            }
            #endregion MailingAddressLine1


            #region MailingAddressLine2
            resultNode = input.SelectSingleNode("//MailingAddressLine2");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:MailingAddressLine2", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlTitleEnglish = response.CreateNode(XmlNodeType.Element, "MailingAddressLine2", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlTitleEnglish.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlTitleEnglish);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:MailingAddressLine2", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }

            }
            #endregion MailingAddressLine2

            #region MailingAddressLine3
            resultNode = input.SelectSingleNode("//MailingAddressLine3");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:MailingAddressLine3", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlTitleEnglish = response.CreateNode(XmlNodeType.Element, "MailingAddressLine3", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlTitleEnglish.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlTitleEnglish);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:MailingAddressLine3", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }
            }
            #endregion MailingAddressLine3


            #region MailingAddressLine4
            resultNode = input.SelectSingleNode("//MailingAddressLine4");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:MailingAddressLine4", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlTitleEnglish = response.CreateNode(XmlNodeType.Element, "MailingAddressLine4", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlTitleEnglish.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlTitleEnglish);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:MailingAddressLine4", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }
            }
            #endregion MailingAddressLine4



            #region MailingAddressLine5
            resultNode = input.SelectSingleNode("//MailingAddressLine5");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:MailingAddressLine5", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlTitleEnglish = response.CreateNode(XmlNodeType.Element, "MailingAddressLine5", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlTitleEnglish.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlTitleEnglish);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:MailingAddressLine5", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }
            }
            #endregion MailingAddressLine5

            #region MailingAddressLine6
            resultNode = input.SelectSingleNode("//MailingAddressLine6");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:MailingAddressLine6", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlTitleEnglish = response.CreateNode(XmlNodeType.Element, "MailingAddressLine6", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlTitleEnglish.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlTitleEnglish);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:MailingAddressLine6", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }
            }
            #endregion MailingAddressLine6


            #region ISOLanguageCode
            resultNode = input.SelectSingleNode("//ISOLanguageCode");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:ISOLanguageCode", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlTitleEnglish = response.CreateNode(XmlNodeType.Element, "ISOLanguageCode", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlTitleEnglish.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlTitleEnglish);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:ISOLanguageCode", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }
            }
            #endregion ISOLanguageCode

            #region PaasportNo
            resultNode = input.SelectSingleNode("//PassportNo");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:PassportNo", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlPassportNo = response.CreateNode(XmlNodeType.Element, "PassportNo", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlPassportNo.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlPassportNo);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:PassportNo", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }
            }
            #endregion ISOLanguageCode

            #region RaceID
            resultNode = input.SelectSingleNode("//RaceID");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:RaceID", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlTitleEnglish = response.CreateNode(XmlNodeType.Element, "RaceID", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlTitleEnglish.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlTitleEnglish);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:RaceID", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }
            }
            #endregion RaceID


            #region MailingAddressPostCode
            resultNode = input.SelectSingleNode("//MailingAddressPostCode");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:MailingAddressPostCode", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlTitleEnglish = response.CreateNode(XmlNodeType.Element, "MailingAddressPostCode", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlTitleEnglish.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlTitleEnglish);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:MailingAddressPostCode", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }
            }
            #endregion MailingAddressPostCode

            #region EmailAddress
            resultNode = input.SelectSingleNode("//EmailAddress");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:email_address", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlTitleEnglish = response.CreateNode(XmlNodeType.Element, "email_address", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlTitleEnglish.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlTitleEnglish);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:email_address", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }
            }
            #endregion EmailAddress

            #region mobile_phone_number
            resultNode = input.SelectSingleNode("//mobile_phone_number");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:mobile_phone_number", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlTitleEnglish = response.CreateNode(XmlNodeType.Element, "mobile_phone_number", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlTitleEnglish.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlTitleEnglish);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:mobile_phone_number", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }
            }
            #endregion mobile_phone_number

            #region DaytimePhoneNumber
            resultNode = input.SelectSingleNode("//daytime_phone_number");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:daytime_phone_number", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlTitleEnglish = response.CreateNode(XmlNodeType.Element, "daytime_phone_number", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlTitleEnglish.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlTitleEnglish);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:daytime_phone_number", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }
            }
            #endregion DaytimePhoneNumber

            #region EveningPhoneNumber
            resultNode = input.SelectSingleNode("//evening_phone_number");
            if (!string.IsNullOrEmpty(Convert.ToString(resultNode)))
            {
                responseNode = response.SelectSingleNode("//def:evening_phone_number", xmlnsManager);
                if (string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    XmlNodeList customerTags = response.GetElementsByTagName("Customer");
                    foreach (XmlNode customerTag in customerTags)
                    {
                        XmlNode xmlTitleEnglish = response.CreateNode(XmlNodeType.Element, "evening_phone_number", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        xmlTitleEnglish.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        customerTag.AppendChild(xmlTitleEnglish);
                    }
                }
                else
                {
                    responseNode.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                }
            }
            else
            {
                responseNode = response.SelectSingleNode("//def:evening_phone_number", xmlnsManager);
                if (!string.IsNullOrEmpty(Convert.ToString(responseNode)))
                {
                    responseNode.ParentNode.RemoveChild(responseNode);
                }
            }
            #endregion EveningPhoneNumber

            #region familydetails
            for (int i = 1; i <= 5; i++)
            {
                resultNode = input.SelectSingleNode("//family_member_" + i + "_dob");
                if (resultNode != null)
                {
                    XmlNode node = response.SelectSingleNode("//def:FamilyDetails[def:FamilyMemberSeqNo='" + i + "']", xmlnsManager);
                    if (node == null)
                    {
                        XmlNode nodes = response.SelectSingleNode("//def:NewDataSet[position() = last()]", xmlnsManager);
                        XmlNode FamilyMember = response.CreateNode(XmlNodeType.Element, "FamilyDetails", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        XmlNode FamilyMemberSeqNo = response.CreateNode(XmlNodeType.Element, "FamilyMemberSeqNo", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        FamilyMemberSeqNo.InnerText = i.ToString();
                        FamilyMember.AppendChild(FamilyMemberSeqNo);
                        XmlNode DateOfBirth = response.CreateNode(XmlNodeType.Element, "DateOfBirth", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        DateOfBirth.InnerText = resultNode.InnerXml;
                        FamilyMember.AppendChild(DateOfBirth);
                        XmlNode number_of_household_members = response.CreateNode(XmlNodeType.Element, "number_of_household_members", "http://tesco.com/clubcardonline/datacontract/2010/01");
                        resultNode = input.SelectSingleNode("//number_of_household_members");
                        number_of_household_members.InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        FamilyMember.AppendChild(number_of_household_members);
                        nodes.AppendChild(FamilyMember);
                    }
                    else
                    {
                        //responseNode = node.SelectSingleNode("//def:DateOfBirth",xmlnsManager);
                        node.ChildNodes[1].InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        resultNode = input.SelectSingleNode("//number_of_household_members");
                        node.ChildNodes[2].InnerText = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                    }
                }
                else
                {
                    XmlNode node = response.SelectSingleNode("//def:FamilyDetails[def:FamilyMemberSeqNo='" + i + "']", xmlnsManager);
                    if (node != null)
                    {
                        node.ParentNode.RemoveChild(node);
                    }
                }
            }
            #endregion familydetails
            utility.WriteXmlFile(CustfileName, response.InnerXml);
            if (clubcardId != null)
            {

                ClubfileName = string.Format(@"DataSource\GetCustomerDetails\{0}\{1}.xml", strCulture, clubcardId);
                ClubfileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ClubfileName);
            }
            WebClient client = new WebClient();
            string contents = client.DownloadString(CustfileName);

            // write contents to test.xml
            System.IO.File.WriteAllText(ClubfileName, contents);


            errorXml = string.Empty;
            customerID = Convert.ToInt32(strCustID);
            return true;
        }

        public bool DeLinkingDotcomAccounts(out string resultXml, string objectXml)
        {
            bool chk = true;
            resultXml = string.Empty;
            string filename = string.Empty;
            XmlDocument response = new XmlDocument();

            try
            {
                XmlDocument input = new XmlDocument();
                input.LoadXml(objectXml);

                XmlNode node = input.SelectSingleNode("//CustomerAlternateID");
                if (node != null)
                {
                    filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("DataSource/DeLinkingDotcomAccounts/result.xml"));
                    string alternateIDFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("DataSource/GetAlternativeIds/{0}.xml", customerID));
                    if (File.Exists(alternateIDFile))
                    {
                        response = utility.LoadXMLDoc(alternateIDFile);
                    }

                    XmlNode responseNode = response.SelectSingleNode("//CusAlterId");
                    responseNode.InnerText = "";
                    utility.WriteXmlFile(alternateIDFile, response.InnerXml);

                }

                resultXml = utility.LoadXMLFile(filename);
            }
            catch (Exception ex)
            {
                chk = false;
                throw ex;
            }
            return chk;
        }

        public bool GetAlternativeIds(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            errorXml = string.Empty;
            rowCount = 0;
            resultXml = string.Empty;
            XmlDocument response = new XmlDocument();
            string fileName = string.Empty;
            XmlDocument input = new XmlDocument();

            try
            {
                input.LoadXml(conditionXml);
                XmlNode custIDNode = input.SelectSingleNode("//CustomerID");
                if (custIDNode != null)
                {
                    customerID = custIDNode.InnerText;
                }
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("DataSource/GetAlternativeIds/{0}.xml", customerID));

                if (File.Exists(fileName))
                {
                    resultXml = utility.LoadXMLFile(fileName);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }

        }
    }

}