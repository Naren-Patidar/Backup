using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using ServiceUtility;
using ClubcardOnline.Web.Entities.CustomerActivationServices;
using System.Data;
using ActivationService.Provider;
using System.ServiceModel;
using System.Globalization;
using ActivationService;

namespace CustomerActivationServices
{
    public class ActivationServiceProvider
    {
        Helper helper = new Helper();
        private ILoggingService _logger = new LoggingService();

        #region Service Methods
        public ClubcardOnline.Web.Entities.CustomerActivationServices.AccountFindByClubcardNumberResponse AccountFindByClubcardNumber(long ClubcardNumber, ClubcardOnline.Web.Entities.CustomerActivationServices.ClubcardCustomer customer, System.Data.DataSet dsConfig)
        {
            string xmlfile = string.Empty;
            bool isDuplicate = false;
            XmlDocument response = new XmlDocument();
            XmlNamespaceManager xmlnsManager = null;
            LogData _logData = new LogData();

            try
            {
                string strNlogConfigPath = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("NLogConfigPath");
                NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strNlogConfigPath), true);
                _logData.CaptureData("ClubcardNumber", ClubcardNumber);
                _logData.CaptureData("CustomerData", customer);
                _logData.CaptureData("ConfigurationData", dsConfig);
                _logger.Submit(_logData);
                AccountDuplicacyCheckerFactory acd = new AccountDuplicacyCheckerFactory();

                var acdChecker = acd.GetInstance(ClubcardNumber);
                if (acdChecker != null)
                {
                    isDuplicate = acdChecker.IsAccountDuplicate(ClubcardNumber, customer, dsConfig);
                }
                _logData.CaptureData("isDuplicate", isDuplicate);
                if (isDuplicate)
                {
                    xmlfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\{0}\{1}", "AccountFindByClubcardNumber", "AccountFindByClubcardNumber_Yes.xml"));
                }
                else
                {
                    xmlfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\{0}\{1}", "AccountFindByClubcardNumber", "AccountFindByClubcardNumber_No.xml"));
                }

                if (File.Exists(xmlfile))
                {
                    response = helper.LoadXMLDoc(xmlfile);
                }

                // create ns manager
                xmlnsManager = new XmlNamespaceManager(response.NameTable);
                xmlnsManager.AddNamespace("def", "http://tempuri.org/");
                xmlnsManager.AddNamespace("b", "http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.WebService.Messages");

                XmlNode xnl = response.SelectSingleNode("//def:AccountFindByClubcardNumberResult", xmlnsManager);
                string xmlString = xnl.InnerXml.Replace("b:", "");
                xmlString = string.Format("<AccountFindByClubcardNumberResponse>{0}</AccountFindByClubcardNumberResponse>", xmlString);
                AccountFindByClubcardNumberResponse accountFindResponse = helper.XMLStringToObject(typeof(AccountFindByClubcardNumberResponse), xmlString) as AccountFindByClubcardNumberResponse;
                _logData.CaptureData("AccountFindResponse", accountFindResponse);
                _logger.Submit(_logData);
                return accountFindResponse;
            }
            catch (Exception ex)
            {
                throw Extensions.GetCustomException("Failed in activation service provider while matching data for customer.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }

        }

        public ClubcardOnline.Web.Entities.CustomerActivationServices.AccountLinkResponse IGHSAccountLink(string dotcomCustomerID, long clubCardNumber)
        {
            XmlDocument response = new XmlDocument();
            string fileName = string.Empty;
            string resultXml = string.Empty;
            string dataFile = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            string xmlFile = string.Empty;
            string alternateIDFilePath = string.Empty;
            XmlNamespaceManager xmlnsManager = null;
            string customerID = string.Empty;
            LogData _logData = new LogData();
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(clubCardNumber)))
                {
                    var custData = helper.SearchByClubcard(clubCardNumber, AppDomain.CurrentDomain.BaseDirectory);
                    _logData.CaptureData("CustomerData", custData);

                    if (!string.IsNullOrEmpty(custData.Item2))
                    {
                        string ServiceName = AppDomain.CurrentDomain.BaseDirectory.Replace("ActivationService", "ClubcardCustomerServices");
                        alternateIDFilePath = Path.Combine(ServiceName, string.Format(@"DataSource\{0}\{1}.xml", "GetAlternativeIds", custData.Item2));

                        if (File.Exists(alternateIDFilePath))
                        {
                            response = helper.LoadXMLDoc(alternateIDFilePath);
                        }

                        XmlNode responseNode = response.SelectSingleNode("//CusAlterId");
                        if (string.IsNullOrEmpty(Convert.ToString(responseNode.InnerText)))
                        {
                            fileName = "LinkAccount.xml";
                            responseNode.InnerText = dotcomCustomerID;
                            helper.WriteXmlFile(alternateIDFilePath, response.InnerXml);
                        }
                        else if (responseNode.InnerText.Equals(dotcomCustomerID))
                        {
                            fileName = "DelinkAccount.xml";
                        }
                    }
                    xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\{0}\{1}", "IGHSAccountLink", fileName));
                }
                if (File.Exists(xmlFile))
                {
                    response = helper.LoadXMLDoc(xmlFile);
                }

                // create ns manager
                xmlnsManager = new XmlNamespaceManager(response.NameTable);
                xmlnsManager.AddNamespace("def", "http://tempuri.org/");
                xmlnsManager.AddNamespace("b", "http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.WebService.Messages");

                XmlNode xml = response.SelectSingleNode("//def:IGHSAccountLinkResult", xmlnsManager);
                string xmlString = xml.InnerXml.Replace("b:", "");
                xmlString = string.Format("<AccountLinkResponse>{0}</AccountLinkResponse>", xmlString);
                AccountLinkResponse accountLinkResponse = helper.XMLStringToObject(typeof(AccountLinkResponse), xmlString) as AccountLinkResponse;
                _logData.CaptureData("AccountLinkResponse", accountLinkResponse);
                _logger.Submit(_logData);

                return accountLinkResponse;
            }
            catch (Exception ex)
            {
                throw Extensions.GetCustomException("Failed in ativation service provider while MCA- delinking the dotcom account.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }


        }
        public bool SendActivationEmail(string strTo)
        {
            bool flag;
            XmlDocument response = new XmlDocument();
            string fileName = string.Empty;
            switch (strTo)
            {
                case "simarpreet_kaur@infosys.com":
                    fileName = string.Format(@"DataSource\{0}\{1}.xml", "SendActivationEmail", "email");
                    break;

                case "neeta.kewlani@in.tesco.com":
                    fileName = string.Format(@"DataSource\{0}\{1}.xml", "SendActivationEmail", "email");
                    break;
            }

            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            response = helper.LoadXMLDoc(fileName);
            XmlNode root = response.DocumentElement;

            // create ns manager
            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
            xmlnsManager.AddNamespace("def", "http://tempuri.org/");


            XmlNode resultNode = response.SelectSingleNode("//def:SendActivationEmailResult", xmlnsManager);
            flag = Convert.ToBoolean((resultNode != null) ? resultNode.InnerXml : string.Empty);
            return flag;

        }
        #endregion
    }
}