using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ServiceUtility
{
    public class Helper
    {
        public XmlDocument LoadXMLDoc(string fileName)
        {
            string outXML = string.Empty;
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            return doc;
        }

        public string LoadXMLFile(string fileName)
        {
            string outXML = string.Empty;
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            outXML = doc.InnerXml;
            return outXML;
        }

        public string LoadTextFile(string fileName)
        {
            string output = string.Empty;
            using (StreamReader sr = new StreamReader(fileName))
            {
                // Read the stream to a string, and write the string to the console.
                output = sr.ReadToEnd();
            }
            return output;
        }

        public string ObjectToXMLString(Type type, object obj)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;

            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, obj);
                }
                return textWriter.ToString();
            }
        }

        public object XMLStringToObject(Type type, string xmlObj)
        {
            var stringReader = new System.IO.StringReader(xmlObj);
            var serializer = new XmlSerializer(type);
            return serializer.Deserialize(stringReader);
        }

        public void WriteXmlFile(string fileName, string content)
        {
            using (StreamWriter file = new System.IO.StreamWriter(fileName, false, Encoding.UTF8))
            {
                file.Write(content);
                file.Close();
            }
        }

        public Tuple<string, string> SearchByClubcard(long ClubcardNumber, string baseDirectory)
        {
            Tuple<string, string> tCustData = new Tuple<string, string>(String.Empty, String.Empty);
            LogData _logData = new LogData();
            try
            {
                var customers = LoadCustomers(baseDirectory);
                var queriedCustomer = customers.Customers.Where(c => c.clubcardNumber.Equals(ClubcardNumber.ToString())).FirstOrDefault();
                if (queriedCustomer != null)
                {
                    _logData.CaptureData("QueriedCustomerData", queriedCustomer);
                    return new Tuple<string, string>(queriedCustomer.Culture, queriedCustomer.customerID);
                }
                else
                {
                    _logData.CaptureData("QueriedCustomerData", queriedCustomer);
                    return new Tuple<string, string>(null, null);
                }
            }
            catch (Exception ex)
            {
                throw Extensions.GetCustomException("Failed in AccountDuplicacycheckFactory while searching culture and customerID by clubcard.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

        public Tuple<string, string> SearchByCustomer(string CustomerID, string baseDirectory)
        {
            Tuple<string, string> tCustData = new Tuple<string, string>(String.Empty, String.Empty);
            LogData _logData = new LogData();
            try
            {
                var customers = LoadCustomers(baseDirectory);
                var queriedCustomer = customers.Customers.Where(c => c.customerID.Equals(CustomerID)).FirstOrDefault();
                if (queriedCustomer != null)
                {
                    _logData.CaptureData("QueriedCustomerData", queriedCustomer);
                    return new Tuple<string, string>(queriedCustomer.Culture, queriedCustomer.clubcardNumber);
                }
                else
                {
                    _logData.CaptureData("QueriedCustomerData", queriedCustomer);
                    return new Tuple<string, string>(null, null);
                }
            }
            catch (Exception ex)
            {
                throw Extensions.GetCustomException("Failed in AccountDuplicacycheckFactory while searching culture and customerID by clubcard.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }

        }

        private static CMetaDataCollection LoadCustomers(string baseDirectory)
        {
            LogData _logData = new LogData();
            string customers = null;
            string ServiceName = string.Empty;
            try
            {
                if (baseDirectory.Contains("ClubcardCustomerServices"))
                {
                    ServiceName = AppDomain.CurrentDomain.BaseDirectory.Replace("ClubcardCustomerServices", "ServiceUtility");
                }
                else if (baseDirectory.Contains("ActivationService"))
                {
                    ServiceName = AppDomain.CurrentDomain.BaseDirectory.Replace("ActivationService", "ServiceUtility");
                }
                string customerData = Path.Combine(ServiceName, string.Format(@"customer.json"));
                customers = File.ReadAllText(customerData);


            }
            catch (Exception ex)
            {
                throw Extensions.GetCustomException("Failed in AccountDuplicacycheckFactory while loading customer data", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return JsonConvert.DeserializeObject<CMetaDataCollection>(customers);

        }
    }
}
