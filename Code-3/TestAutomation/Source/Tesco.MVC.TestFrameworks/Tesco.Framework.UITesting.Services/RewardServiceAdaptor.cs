using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Services.RewardService;
using System.Collections;
using System.Xml.Linq;
using System.Xml;
using System.Linq;


namespace Tesco.Framework.UITesting.Services
{
    public class RewardServiceAdaptor
    {
        public List<Reward> GetRewardDetails(long customerId, string culture)
        {
            DateTime dtTemp = new DateTime();
            List<Reward> _reward = new List<Reward>();
            string errorXml, resultXml;
            XDocument xDoc = new XDocument();
            RewardServiceAdaptor rewardAdaptor = new RewardServiceAdaptor();
            using (RewardServiceClient client = new RewardServiceClient())
            {
                client.GetRewardDetail(out errorXml, out resultXml, customerId, culture);
                XmlDocument resulDoc = new XmlDocument();
                resulDoc.LoadXml(resultXml);
                xDoc = XDocument.Parse(resultXml);
               // DateTime dt = new DateTime();


                _reward = (from t in xDoc.Descendants("RewardDetails")
                           select new Reward

                           {
                               BookingDate = t.Element("BookingDate").GetValue<string>().Substring(0, 10),
                               TokenDescription = t.Element("TokenDescription").GetValue<string>(),
                               ProductStatus = t.Element("ProductStatus").GetValue<string>(),
                               TokenValue = Convert.ToDecimal(t.Element("TokenValue").GetValue<string>()),
                               SupplierTokenCode = t.Element("SupplierTokenCode").GetValue<string>(),
                               ValidUntil = t.Element("ValidUntil").GetValue<string>().Substring(0, 10)

                           }).ToList();    
               
               
            }
            return _reward;
            
            
        }


        public List<Reward> GetTokenDetails(long customerId, string culture)
        {
           
            List<Reward> _reward = new List<Reward>();
            string errorXml, resultXml;
            XDocument xDoc = new XDocument();
            RewardServiceAdaptor rewardAdaptor = new RewardServiceAdaptor();
            using (RewardServiceClient client = new RewardServiceClient())
            {
                client.GetRewardDetail(out errorXml, out resultXml, customerId, culture);
                XmlDocument resulDoc = new XmlDocument();
                resulDoc.LoadXml(resultXml);
                xDoc = XDocument.Parse(resultXml);
                // DateTime dt = new DateTime();


                _reward = (from t in xDoc.Descendants("TokenDetails")
                           select new Reward

                           {
                               BookingDate = t.Element("BookingDate").GetValue<string>().Substring(0, 10),
                               TokenDescription = t.Element("TokenDescription").GetValue<string>(),
                               ProductStatus = t.Element("ProductStatus").GetValue<string>(),
                               TokenValue = Convert.ToDecimal(t.Element("TokenValue").GetValue<string>()),
                               SupplierTokenCode = t.Element("SupplierTokenCode").GetValue<string>(),
                               ValidUntil = t.Element("ValidUntil").GetValue<string>().Substring(0, 10),
                               ProductCode = t.Element("ProductCode").GetValue<string>(),
                               ProductTokenValue = t.Element("ProductTokenValue").GetValue<long>(),
                               QualifyingSpend = t.Element("QualifyingSpend").GetValue<string>(),
                               Title = t.Element("Title").GetValue<string>(),

                        
                         
                          

                      }).ToList();

                         


            }
            return _reward;


        }
    }
    public class Reward
        {
            public string BookingDate { get; set; }
            public string TokenDescription { get; set; }
            public string ProductStatus { get; set; }
            public decimal TokenValue { get; set; }
            public string SupplierTokenCode { get; set; }
            public string ValidUntil { get; set; }
            public string ProductCode{get;set;}

            public string QualifyingSpend { get; set; }
            public long ProductTokenValue { get; set; }
            public string Title { get; set; }

        }
}