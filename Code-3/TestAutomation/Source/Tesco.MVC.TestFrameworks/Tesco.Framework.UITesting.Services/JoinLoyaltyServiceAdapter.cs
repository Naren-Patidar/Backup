using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Services.JoinLoyaltyService;
using System.Collections;
using System.Xml.Linq;

namespace Tesco.Framework.UITesting.Services
{
    public class JoinLoyaltyServiceAdapter
    {
        //public long People(string emailadress)
        //{
        //    long id = 0;
        //    using (JoinLoyaltyServiceClient client = new JoinLoyaltyServiceClient())
        //    {
        //        Hashtable searchData = new Hashtable();
        //        searchData["EmailAddress"] = emailadress;
        //        string errorXml = string.Empty, resultXml = string.Empty;
        //        string conditionXML = Utility.HashTableToXML(searchData, "EmailAddress");
        //        if (client.
        //            XDocument xDoc = XDocument.Parse(resultXml);
        //            List<long> ids = (from t in xDoc.Descendants("EmailAddress")
        //                              select t.Element("NumberOfPeople").GetValue<long>()).ToList();
        //            if (ids.Count > 0)
        //            {
        //                id = ids.FirstOrDefault();
        //            }
        //        }
        //    }
        //    return id;
        //}
    }

}

