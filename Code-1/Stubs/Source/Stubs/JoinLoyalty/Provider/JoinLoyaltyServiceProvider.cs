using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using ServiceUtility;

namespace JoinLoyaltyService
{
    public class JoinLoyaltyServiceProvider
    {
        Helper helper = new Helper();
        #region Service Methods
        public bool AccountDuplicateCheck(out string resultXml, string inputXml)
        {
            bool chk = true;
            XmlDocument doc = new XmlDocument();
            string xmlFile = string.Empty;
            string promotionCode = string.Empty;
            string promoFile = string.Empty;
            string dataFile=string.Empty;
            bool isDuplicate = false;
            bool ispromovalid = false;
            XmlDocument xmlDoc = new XmlDocument();
             XmlDocument xmlDataDoc = new XmlDocument();
            resultXml = string.Empty;
            string queryData=string.Empty;
            try
            {
                doc.LoadXml(inputXml);
                if (doc != null)
                {
                    XmlDocument response = new XmlDocument();
                    XmlNode root = response.DocumentElement;

                    Dictionary<string, string> userInput = new Dictionary<string, string>();
                    userInput = InputXmlToList(inputXml);
                    if (userInput.ContainsKey("PromotionCode"))
                    {
                        if (!string.IsNullOrEmpty(userInput["PromotionCode"]))
                        {
                            promoFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\AccountDuplicateCheck\PromoCodes.xml"));
                            string query = "PromotionCodes/Code";
                            xmlDoc.Load(promoFile);
                            foreach (XmlNode node in xmlDoc.SelectNodes(query))
                            {
                                if (userInput["Culture"].Equals(node["Culture"].InnerText.Trim().ToLower()))
                                {
                                    if (userInput["PromotionCode"].Equals(node["PromotionCode"].InnerText.Trim().ToLower()))
                                    {
                                        ispromovalid = Convert.ToBoolean(node["IsValid"].InnerText.Trim().ToLower());
                                    }

                                }
                            }
                            if (!ispromovalid)
                            {
                                xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\{0}\{1}", "AccountDuplicateCheck", "InvalidPromo.xml"));
                            }
                        }
                        if (string.IsNullOrEmpty(userInput["PromotionCode"]) || ispromovalid)
                        {
                             queryData = "AccountData/Data";
                            dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\AccountDuplicateCheck\DuplicacyCheckData.xml"));
                            xmlDataDoc.Load(dataFile);
                            foreach (XmlNode node in xmlDataDoc.SelectNodes(queryData))
                            {
                                if (userInput["Culture"].Equals(node["Culture"].InnerText.Trim().ToLower()))
                                {
                                    if (string.IsNullOrEmpty(userInput["MailingAddressPostCode"]))
                                    {
                                        node["MailingAddressPostCode"].InnerText = userInput["MailingAddressPostCode"];
                                    }
                                    if (string.IsNullOrEmpty(userInput["Name1"]))
                                    {
                                        node["Name1"].InnerText = userInput["Name1"];
                                    }
                                    if (string.IsNullOrEmpty(userInput["Name2"]))
                                    {
                                        node["Name2"].InnerText = userInput["Name2"];
                                    }
                                    if (string.IsNullOrEmpty(userInput["Name3"]))
                                    {
                                        node["Name3"].InnerText = userInput["Name3"];
                                    }
                                    if (string.IsNullOrEmpty(userInput["MailingAddressLine1"]))
                                    {
                                        node["MailingAddressLine1"].InnerText = userInput["MailingAddressLine1"];
                                    }
                                    if (string.IsNullOrEmpty(userInput["EmailAddress"]))
                                    {
                                        node["EmailAddress"].InnerText = userInput["EmailAddress"];
                                    }
                                    if (userInput["MailingAddressPostCode"].Equals(node["MailingAddressPostCode"].InnerText.Trim().ToLower()) &&
                                             userInput["Name1"].Equals(node["Name1"].InnerText.Trim().ToLower()) &&
                                              userInput["Name2"].Equals(node["Name2"].InnerText.Trim().ToLower()) &&
                                             userInput["Name3"].Equals(node["Name3"].InnerText.Trim().ToLower()) &&
                                             userInput["MailingAddressLine1"].Equals(node["MailingAddressLine1"].InnerText.Trim().ToLower()) &&
                                             userInput["EmailAddress"].Equals(node["EmailAddress"].InnerText.Trim().ToLower()))
                                    {
                                        isDuplicate = Convert.ToBoolean(node["isDuplicate"].InnerText.Trim().ToLower());
                                    }
                                }


                            }
                            if (isDuplicate)
                            {
                                xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\{0}\{1}", "AccountDuplicateCheck", "AccountDuplicate.xml"));
                            }
                            else
                            {
                                xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\{0}\{1}", "AccountDuplicateCheck", "ValidJoinData.xml"));
                            }
                        }
                    }
                        else
                        {
                             queryData = "AccountData/Data";
                            dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\AccountDuplicateCheck\DuplicacyCheckData.xml"));
                            xmlDataDoc.Load(dataFile);
                            foreach (XmlNode node in xmlDataDoc.SelectNodes(queryData))
                            {
                                if (userInput["Culture"].Equals(node["Culture"].InnerText.Trim().ToLower()))
                                {
                                    if (string.IsNullOrEmpty(userInput["MailingAddressPostCode"]))
                                    {
                                        node["MailingAddressPostCode"].InnerText = userInput["MailingAddressPostCode"];
                                    }
                                    if (string.IsNullOrEmpty(userInput["Name1"]))
                                    {
                                        node["Name1"].InnerText = userInput["Name1"];
                                    }
                                    if (string.IsNullOrEmpty(userInput["Name2"]))
                                    {
                                        node["Name2"].InnerText = userInput["Name2"];
                                    }
                                    if (string.IsNullOrEmpty(userInput["Name3"]))
                                    {
                                        node["Name3"].InnerText = userInput["Name3"];
                                    }
                                    if (string.IsNullOrEmpty(userInput["MailingAddressLine1"]))
                                    {
                                        node["MailingAddressLine1"].InnerText = userInput["MailingAddressLine1"];
                                    }
                                    if (string.IsNullOrEmpty(userInput["EmailAddress"]))
                                    {
                                        node["EmailAddress"].InnerText = userInput["EmailAddress"];
                                    }
                                    if (userInput["MailingAddressPostCode"].Equals(node["MailingAddressPostCode"].InnerText.Trim().ToLower()) &&
                                             userInput["Name1"].Equals(node["Name1"].InnerText.Trim().ToLower()) &&
                                              userInput["Name2"].Equals(node["Name2"].InnerText.Trim().ToLower()) &&
                                             userInput["Name3"].Equals(node["Name3"].InnerText.Trim().ToLower()) &&
                                             userInput["MailingAddressLine1"].Equals(node["MailingAddressLine1"].InnerText.Trim().ToLower()) &&
                                             userInput["EmailAddress"].Equals(node["EmailAddress"].InnerText.Trim().ToLower()))
                                    {
                                        isDuplicate = Convert.ToBoolean(node["isDuplicate"].InnerText.Trim().ToLower());
                                    }
                                }

                            }
                            if (isDuplicate)
                            {
                                xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\{0}\{1}", "AccountDuplicateCheck", "AccountDuplicate.xml"));
                            }
                            else
                            {
                                xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\{0}\{1}", "AccountDuplicateCheck", "ValidJoinData.xml"));
                            }
                    }
                        
                    if (File.Exists(xmlFile))
                    {
                        response = helper.LoadXMLDoc(xmlFile);
                       

                    }
                    //  create ns manager
                    XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
                    xmlnsManager.AddNamespace("def", "http://tempuri.org/");
                    XmlNode resultNode = response.SelectSingleNode("//def:resultXml", xmlnsManager);
                    resultXml = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                    resultXml = resultXml.Replace("xmlns=\"http://tempuri.org/\"", "");

                }
            }
            catch (Exception ex)
            {
                chk = false;
                throw ex;
            }
            return chk;
        }

        public string AccountCreate(long dotcomCustomerID, string objectXml, string source, string culture)
        {
            string resultXml = string.Empty;
            XmlDocument doc = new XmlDocument();
            string xmlFile = string.Empty;

            try
            {
                doc.LoadXml(objectXml);
                if (doc != null)
                {
                    xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource/AccountCreate/{0}/{1}.xml", culture, dotcomCustomerID));
                    XmlDocument response = helper.LoadXMLDoc(xmlFile);

                    XmlNode root = response.DocumentElement;

                    // create ns manager
                    XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
                    xmlnsManager.AddNamespace("def", "http://tempuri.org/");


                    XmlNode resultNode = response.SelectSingleNode("//def:resultXml", xmlnsManager);
                    resultXml = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                    resultXml = resultXml.Replace("xmlns=\"http://tempuri.org/\"", "");

                }
            }
            catch (Exception ex)
            {
                resultXml = null;
                throw ex;
            }
            return resultXml;
        }

        public Dictionary<string, string> InputXmlToList(string file)
        {
            try
            {
                Dictionary<string, string> words = new Dictionary<string, string>();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(file);

                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    words.Add(node.Name, node.InnerText.Trim().ToLower());
                }
                return words;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
    }
}