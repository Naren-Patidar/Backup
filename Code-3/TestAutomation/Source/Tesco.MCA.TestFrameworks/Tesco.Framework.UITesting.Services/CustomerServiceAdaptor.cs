using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Services.CustomerService;
using Tesco.Framework.UITesting;
using System.Collections;
using System.Xml.Linq;
using System.Linq;
using System.Data;
using System.Web;
using System.Xml;


namespace Tesco.Framework.UITesting.Services
{
    public class CustomerServiceAdaptor
    {

        public Int64 GetHouseholdID(string customerId, string strCulture)
        {
            Int64 id = 0;
            using (CustomerServiceClient client = new CustomerServiceClient())
            {
                Hashtable searchData = new Hashtable();
                searchData["CustomerID"] = customerId;
                string errorXml = string.Empty, resultXml = string.Empty;
                int rowCount = 0;
                string conditionXML = Utility.HashTableToXML(searchData, "customer");
                if (client.SearchCustomer(out errorXml, out resultXml, out rowCount, conditionXML, 1, strCulture))
                {

                    if (resultXml.Contains("HouseHoldID"))
                    {
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(resultXml);
                        DataSet dsCustomerInfo = new DataSet();
                        dsCustomerInfo.ReadXml(new XmlNodeReader(xDoc));
                        //XDocument xDoc = XDocument.Parse(resultXml);
                        //List<Int64> ids = (from t in xDoc.Descendants("Customer")
                        //                   select t.Element("HouseHoldID").GetValue<Int64>()).ToList();

                        id = Convert.ToInt64(dsCustomerInfo.Tables["Customer"].Rows[0]["HouseHoldID"]);
                    }

                    //List<Int64> ids = (from t in xDoc.Descendants("Customer")
                    //                     select t.Element("HouseHoldID").GetValue<Int64>()).ToList();

                    //long HouseholdID = Convert.ToInt64(xDoc.Root.Element("HouseHoldID"));
                    // long hhid=xDoc.h

                }
            }
            return id;

        }

        public Int64 GetCustomerID(string clubcardNumber, string strCulture)
        {
            Int64 id = 0;
            using (CustomerServiceClient client = new CustomerServiceClient())
            {

                Hashtable searchData = new Hashtable();
                searchData["cardAccountNumber"] = clubcardNumber;
                string errorXml = string.Empty, resultXml = string.Empty;
                int rowCount = 0;
                string conditionXML = Utility.HashTableToXML(searchData, "customer");
                if (client.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, 1, strCulture))
                {
                    XDocument xDoc = XDocument.Parse(resultXml);
                    List<Int64> ids = (from t in xDoc.Descendants("Customer")
                                       select t.Element("CustomerID").GetValue<Int64>()).ToList();
                    if (ids.Count > 0)
                    {
                        id = ids.FirstOrDefault();
                    }
                }
            }
            return id;
        }

        public List<string> GetProvince(string clubcardNumber, string strCulture,Enums.CustPersonalDetail culture)
        {
            List<string> province=null;
            using (CustomerServiceClient client = new CustomerServiceClient())
            {

                Hashtable searchData = new Hashtable();
                searchData["cardAccountNumber"] = clubcardNumber;
                string errorXml = string.Empty, resultXml = string.Empty;
                int rowCount = 0;
                string conditionXML = Utility.HashTableToXML(searchData, "customer");
                switch(culture)
                {
                 case Enums.CustPersonalDetail.ProvinceLocale:
                 if (client.GetConfigDetails(out errorXml, out resultXml, out rowCount, "7,", strCulture))
                  {
                    XDocument xDoc = XDocument.Parse(resultXml);
                     province = (from t in xDoc.Descendants("Table3")
                                       select t.Element("provincenamelocal").GetValue<string>()).ToList();
                  }
                break;
                  case Enums.CustPersonalDetail.ProvinceEnglish:
                  if (client.GetConfigDetails(out errorXml, out resultXml, out rowCount, "7,", strCulture))
                    {
                    XDocument xDoc = XDocument.Parse(resultXml);
                     province = (from t in xDoc.Descendants("Table3")
                                       select t.Element("provincenameEnglish").GetValue<string>()).ToList();
                    }
                break;
                  case Enums.CustPersonalDetail.RaceLocale:
                if (client.GetConfigDetails(out errorXml, out resultXml, out rowCount, "7,", strCulture))
                {
                    XDocument xDoc = XDocument.Parse(resultXml);
                    province = (from t in xDoc.Descendants("Table1")
                                select t.Element("racedesclocal").GetValue<string>().Trim()).ToList();
                }
                break;
                  case Enums.CustPersonalDetail.RaceEnglish:
                if (client.GetConfigDetails(out errorXml, out resultXml, out rowCount, "7,", strCulture))
                {
                    XDocument xDoc = XDocument.Parse(resultXml);
                    province = (from t in xDoc.Descendants("Table1")
                                select t.Element("racedescenglish").GetValue<string>()).ToList();
                }
                break;
            }
            }
            return province;
        }
        public string GetEmailId(string customerId, string strCulture)
        {
            string EmailId = string.Empty;
            using (CustomerServiceClient client = new CustomerServiceClient())
            {

                Hashtable searchData = new Hashtable();
                searchData["CustomerID"] = customerId;
                string errorXml = string.Empty, resultXml = string.Empty;
                int rowCount = 0;
                string conditionXML = Utility.HashTableToXML(searchData, "customer");
                if (client.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, 100, strCulture))
                {
                    XDocument xDoc = XDocument.Parse(resultXml);
                    List<string> EmailIds = (from t in xDoc.Descendants("Customer")
                                             select t.Element("email_address").GetValue<String>()).ToList();
                    if (EmailIds.Count > 0)
                    {
                        EmailId = EmailIds.FirstOrDefault();
                    }
                }
                return EmailId;
            }
        }

        public string GetPostCode(Int64 customerId, string strCulture)
        {
            string PostCode = string.Empty;
            using (CustomerServiceClient client = new CustomerServiceClient())
            {

                Hashtable searchData = new Hashtable();
                searchData["CustomerID"] = customerId;
                string errorXml = string.Empty, resultXml = string.Empty;
                int rowCount = 0;
                string conditionXML = Utility.HashTableToXML(searchData, "customer");
                if (client.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, 100, strCulture))
                {
                    XDocument xDoc = XDocument.Parse(resultXml);
                    List<string> Postcode = (from t in xDoc.Descendants("Customer")
                                             select t.Element("postal_code").GetValue<String>()).ToList();
                    if (Postcode.Count > 0)
                    {
                        PostCode = Postcode.FirstOrDefault();
                    }
                }
                return PostCode;
            }
        }

        public string GetPhoneNumber(string customerId, string strCulture)
        {
            string phoneNumber = string.Empty;
            // Int64 id = GetCustomerID(customerId, strCulture);
            using (CustomerServiceClient client = new CustomerServiceClient())
            {

                Hashtable searchData = new Hashtable();
                searchData["CustomerID"] = customerId;
                string errorXml = string.Empty, resultXml = string.Empty;
                int rowCount = 0;
                string conditionXML = Utility.HashTableToXML(searchData, "customer");
                if (client.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, 100, strCulture))
                {
                    XDocument xDoc = XDocument.Parse(resultXml);
                    List<string> phoneNumbers = (from t in xDoc.Descendants("Customer")
                                                 select t.Element("mobile_phone_number").GetValue<String>()).ToList();
                    if (phoneNumbers.Count > 0)
                    {
                        phoneNumber = phoneNumbers.FirstOrDefault();
                    }
                }
                return phoneNumber;
            }
        }

        public string GetEmailIdForJoin(string strCulture)
        {
            string EmailId = string.Empty;
            using (CustomerServiceClient client = new CustomerServiceClient())
            {

                Hashtable searchData = new Hashtable();
                searchData["Sex"] = "M";
                string errorXml = string.Empty, resultXml = string.Empty;
                int rowCount = 0;
                string conditionXML = Utility.HashTableToXML(searchData, "sex");
                if (client.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, 100, strCulture))
                {
                    XDocument xDoc = XDocument.Parse(resultXml);
                    List<string> EmailIds = (from t in xDoc.Descendants("Customer")
                                             select t.Element("email_address").GetValue<String>()).ToList();
                    if (EmailIds.Count > 0)
                    {
                        EmailId = EmailIds.FirstOrDefault();
                    }
                }
                return EmailId;
            }
        }


        public Dictionary<string, string> GetCustomerDetails(string customerId, string strCulture)
        {
            Dictionary<string, string> customerDetails = new Dictionary<string, string>();
            // Int64 id = GetCustomerID(customerId, strCulture);
            using (CustomerServiceClient client = new CustomerServiceClient())
            {

                Hashtable searchData = new Hashtable();
                searchData["CustomerID"] = customerId;
                string errorXml = string.Empty, resultXml = string.Empty;
                int rowCount = 0;
                string conditionXML = Utility.HashTableToXML(searchData, "customer");
                if (client.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, 100, strCulture))
                {
                    XDocument xDoc = XDocument.Parse(resultXml);
                    customerDetails = (from t in xDoc.Descendants("Customer").First().Descendants()
                                       select new
                                       {
                                           Name = t.Name.LocalName,
                                           Value = t.GetValue<String>(),
                                       }).ToDictionary(o => o.Name, o => o.Value);


                }
                return customerDetails;
            }


        }

        public List<string> GetFamilyDetails(string customerId, string strCulture)
        {
            List<string> FamilyDetails = new List<string>();
            DateTime tmp = DateTime.Now;
            // Int64 id = GetCustomerID(customerId, strCulture);
            using (CustomerServiceClient client = new CustomerServiceClient())
            {

                Hashtable searchData = new Hashtable();
                searchData["CustomerID"] = customerId;
                string errorXml = string.Empty, resultXml = string.Empty;
                int rowCount = 0;
                string conditionXML = Utility.HashTableToXML(searchData, "customer");
                if (client.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, 100, strCulture))
                {
                    XDocument xDoc = XDocument.Parse(resultXml);
                    FamilyDetails = (from t in xDoc.Descendants("FamilyDetails")
                                     select t.Element("DateOfBirth").GetValue<string>().TryParseDate(out tmp) ? tmp.Year.ToString() : string.Empty).ToList();
                }
            }
            return FamilyDetails;

        }
        public string GetFamilyMember1Age(string customerId, string strCulture)
        {
            string Familymemberage = string.Empty;
            DateTime tmp = DateTime.Now;
            
            using (CustomerServiceClient client = new CustomerServiceClient())
            {

                Hashtable searchData = new Hashtable();
                searchData["CustomerID"] = customerId;
                string errorXml = string.Empty, resultXml = string.Empty;
                int rowCount = 0;
                string conditionXML = Utility.HashTableToXML(searchData, "customer");
                if (client.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, 100, strCulture))
                {

                    XDocument xDoc = XDocument.Parse(resultXml);
                    List<string> tmpList = (from t in xDoc.Descendants("Customer")
                                            select t.Element("family_member_1_dob").GetValue<string>().TryParseDate(out tmp) ? tmp.Year.ToString() : string.Empty).ToList();
                    Familymemberage = tmpList.FirstOrDefault();
                    Familymemberage = string.IsNullOrEmpty(Familymemberage) ? "AgeNotSelected" : Familymemberage;
                }
            }
            return Familymemberage;
        }
        public string GetCustomerName(string customerId, string strCulture)
        {
            string CustomerName = string.Empty;
            using (CustomerServiceClient client = new CustomerServiceClient())
            {

                Hashtable searchData = new Hashtable();
                searchData["CustomerID"] = customerId;
                string errorXml = string.Empty, resultXml = string.Empty;
                int rowCount = 0;
                string conditionXML = Utility.HashTableToXML(searchData, "customer");
                if (client.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, 100, strCulture))
                {
                    XDocument xDoc = XDocument.Parse(resultXml);
                    CustomerName = (from t in xDoc.Descendants("Customer")
                                    select t.Element("Name").GetValue<string>()).ToString();
                }
            }
            return CustomerName;
        }

        public DateTime GetCutoffDate(string configurationTypes, string locale)
        { 
           List<string> dates = new List<string>();
            DateTime CutOffDates = DateTime.Now;
            DateTime tmp =  DateTime.Now;
            string errorXml = string.Empty, resultXml = string.Empty;
                int rowCount = 0;
            CustomerServiceClient client = new CustomerServiceClient();
            if (client.GetConfigDetails(out errorXml, out resultXml, out rowCount, configurationTypes, locale))
            {
                XDocument xDoc = XDocument.Parse(resultXml);
                List<DateTime> tempdates = (from t in xDoc.Descendants("ActiveDateRangeConfig")
                                            select t.Element("ConfigurationValue1").GetValue<DateTime>()).ToList();

                CutOffDates = tempdates.FirstOrDefault();
            }

            return CutOffDates;   
        }
        public DateTime GetSignoffDate(string customerId, string locale)
        {
            List<string> dates = new List<string>();
            DateTime signoffDates = DateTime.Now;
            DateTime tmp = DateTime.Now;
            string errorXml = string.Empty, resultXml = string.Empty;
            int rowCount = 0;
            CustomerServiceClient client = new CustomerServiceClient();
            if (client.GetConfigDetails(out errorXml, out resultXml, out rowCount, customerId, locale))
            {
                XDocument xDoc = XDocument.Parse(resultXml);
                List<DateTime> tempdates = (from t in xDoc.Descendants("ActiveDateRangeConfig")
                                            select t.Element("ConfigurationValue2").GetValue<DateTime>()).ToList();

                signoffDates = tempdates.FirstOrDefault();
            }

            return signoffDates;
        }
        public string GetCustomerMailStatus(string customerId, string strCulture)
        {
            string CustomerMailStatus = string.Empty;
            using (CustomerServiceClient client = new CustomerServiceClient())
            {

                Hashtable searchData = new Hashtable();
                searchData["CustomerID"] = customerId;
                string errorXml = string.Empty, resultXml = string.Empty;
                int rowCount = 0;
                string conditionXML = Utility.HashTableToXML(searchData, "customer");
                if (client.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, 100, strCulture))
                {
                    XDocument xDoc = XDocument.Parse(resultXml);
                    CustomerMailStatus = (from t in xDoc.Descendants("Customer")
                                          select t.Element("CustomerMailStatus").GetValue<string>()).ToString();
                }
            }
            return CustomerMailStatus;
        }
    }
}

