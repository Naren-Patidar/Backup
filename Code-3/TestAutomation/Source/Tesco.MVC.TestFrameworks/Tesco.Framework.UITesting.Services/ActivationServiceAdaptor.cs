using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Services.ActivationService;

namespace Tesco.Framework.UITesting.Services
{
    public class ActivationServiceAdaptor
    {

        public string GetAlternateIDs(string customerId, string strCulture)
        {             
            string CustomerMailStatus = string.Empty;
            //using (ClubcardOnlineServiceClient client = new ClubcardOnlineServiceClient())
            //{

            //    Hashtable searchData = new Hashtable();
            //    searchData["CustomerID"] = customerId;
            //    string errorXml = string.Empty, resultXml = string.Empty;
            //    int rowCount = 0;
            //    string conditionXML = Utility.HashTableToXML(searchData, "customer");
            //    if (client.Geta(out errorXml, out resultXml, out rowCount, conditionXML, 100, strCulture))
            //    {
            //        XDocument xDoc = XDocument.Parse(resultXml);
            //        CustomerMailStatus = (from t in xDoc.Descendants("Customer")
            //                              select t.Element("CustomerMailStatus").GetValue<string>()).ToString();
            //    }
            //}
            return CustomerMailStatus;
        }
    }
}
