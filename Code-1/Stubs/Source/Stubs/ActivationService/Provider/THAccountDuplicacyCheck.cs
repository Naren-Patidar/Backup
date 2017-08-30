using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using ServiceUtility;
using System.Xml;
using System.Data;

namespace ActivationService.Provider
{
    public class THAccountDuplicacyCheck : AccountDuplicacyCheck
    {
        private ILoggingService _logger = new LoggingService();
        Helper helper = new Helper();
        public override bool IsAccountDuplicate(long ClubcardNumber, ClubcardOnline.Web.Entities.CustomerActivationServices.ClubcardCustomer customer, System.Data.DataSet dsConfig)
        {
            LogData _logData = new LogData();
            string fileName = string.Empty;
            XmlDocument response = new XmlDocument();
            XmlNamespaceManager xmlnsManager = null;
            XmlNode responseNode = null;
            string yearOfBirth = string.Empty;
            string MonthOfBirth = string.Empty;
            string DayOfBirth = string.Empty;
            string customerDateOfBirth = string.Empty;
            DateTime dtResult = DateTime.MinValue;

            try
            {

                string ServiceName = AppDomain.CurrentDomain.BaseDirectory.Replace("ActivationService", "ClubcardCustomerServices");
                fileName = Path.Combine(ServiceName, string.Format(@"DataSource\GetCustomerDetails\th-TH\{0}.xml", ClubcardNumber));
                _logData.CaptureData("FileName", fileName);
                //  fileName = string.Format(@"D:\MCA_RefactoredCode\UK\Stub_Jan2016\ClubcardService\DataSource\GetCustomerDetails\th-TH\{0}.xml", ClubcardNumber);
                if (File.Exists(fileName))
                {
                    response = helper.LoadXMLDoc(fileName);
                }

                xmlnsManager = new XmlNamespaceManager(response.NameTable);
                xmlnsManager.AddNamespace("def", "http://tesco.com/clubcardonline/datacontract/2010/01");
                foreach (DataRow value in dsConfig.Tables[0].Rows)
                {
                    if (value["ConfigurationName"].ToString().Trim().ToLower().Equals("mobilephonenumber"))
                    {
                        responseNode = response.SelectSingleNode("//def:mobile_phone_number", xmlnsManager);
                    }
                    else if (value["ConfigurationName"].ToString().Trim().ToLower().Contains("birth"))
                    {
                        if (string.IsNullOrEmpty(customerDateOfBirth))
                        {
                            responseNode = response.SelectSingleNode("//def:family_member_1_dob", xmlnsManager);
                            customerDateOfBirth = responseNode.InnerText;

                            if (DateTime.TryParse(customerDateOfBirth, out dtResult))
                            {
                                DayOfBirth = dtResult.Day.ToString();
                                MonthOfBirth = dtResult.Month.ToString();
                                yearOfBirth = dtResult.Year.ToString();
                            }
                            else
                            {
                                yearOfBirth = responseNode.InnerText.Substring(0, 4).Trim().ToLower();
                                MonthOfBirth = responseNode.InnerText.Substring(5, 2).Trim().ToLower();
                                DayOfBirth = responseNode.InnerText.Substring(8, 2).Trim().ToLower();
                            }
                        }
                    }
                    else
                    {
                        responseNode = response.SelectSingleNode("//def:" + value["ConfigurationName"], xmlnsManager);
                    }
                    if (responseNode.Name != null)
                    {
                        switch (value["ConfigurationName"].ToString())
                        {
                            case "SSN":
                                if (!(responseNode.InnerText.Trim().ToLower() == customer.SSN.Trim().ToLower()))
                                {
                                    return false;
                                }
                                break;
                            case "YearofBirth":
                                if (!(yearOfBirth == customer.YearOfBirth.Trim().ToLower()))
                                {
                                    return false;
                                }
                                break;
                            case "MonthofBirth":
                                if (!(MonthOfBirth == customer.MonthOfBirth.Trim().ToLower()))
                                {
                                    return false;
                                }
                                break;
                            case "DayofBirth":
                                if (!(DayOfBirth == customer.DayOfBirth.Trim().ToLower()))
                                {
                                    return false;
                                }
                                break;
                            case "MobilePhoneNumber":
                                if (!(responseNode.InnerText.Trim().ToLower() == customer.ContactDetail.MobileContactNumber.Trim().ToLower()))
                                {
                                    return false;
                                }
                                break;
                            default:
                                return false;
                        }
                    }
                }
                _logger.Submit(_logData);
                return true;
            }
            catch (Exception ex)
            {
                throw Extensions.GetCustomException("Failed in THAccountDuplicacy check while matching CustomerData.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }
    }
}