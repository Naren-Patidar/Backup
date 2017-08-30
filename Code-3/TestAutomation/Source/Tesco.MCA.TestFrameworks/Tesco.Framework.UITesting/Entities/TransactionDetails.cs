using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Tesco.Framework.UITesting.Helpers;

namespace Tesco.Framework.UITesting.Entities
{
    public class TransactionDetails
    {
        public string ClubCardTransactionId { get; set; }
        public string ClubcardId { get; set; }
        public string ClubCardStatusDescEnglish { get; set; }
        public string CustType { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string PartnerId { get; set; }
        public string TescoStoreId { get; set; }
        public string AmountSpent { get; set; }
        public string NormalPoints { get; set; }
        public string BonusPoints { get; set; }
        public string TotalPoints { get; set; }
        public string TransactionDescription { get; set; }
        public string PointIssuePartnerGroupId { get; set; }
        public string PointIssuePartnerGroupDesc { get; set; }

        public static List<TransactionDetails> ConvertFromXml(string resultXml)
        {
            List<TransactionDetails> transactions = new List<TransactionDetails>();
            DateTime dtTemp = DateTime.Now;
            XDocument xDoc = XDocument.Parse(resultXml);
            transactions = (from t in xDoc.Descendants("Transactions")
                                   select new TransactionDetails
                                   {
                                       ClubCardTransactionId = t.Element("ClubcardTransactionID").GetValue<string>(),
                                       ClubcardId = t.Element("ClubcardID").GetValue<string>(),
                                       ClubCardStatusDescEnglish = t.Element("ClubcardStatusDescEnglish").GetValue<string>(),
                                       CustType = t.Element("CustType").GetValue<string>(),
                                       TransactionDateTime = t.Element("TransactionDateTime").GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.Now,
                                       PartnerId = t.Element("PartnerId").GetValue<string>(),
                                       TescoStoreId = t.Element("TescoStoreId").GetValue<string>(),
                                       AmountSpent = t.Element("AmountSpent").GetValue<string>(),
                                       NormalPoints = t.Element("NormalPoints").GetValue<string>(),
                                       BonusPoints = t.Element("BonusPoints").GetValue<string>(),
                                       TotalPoints = t.Element("TotalPoints").GetValue<string>(),
                                       TransactionDescription = t.Element("TransactionDescription").GetValue<string>(),
                                       PointIssuePartnerGroupId = t.Element("PointIssuePartnerGroupId").GetValue<string>(),
                                       PointIssuePartnerGroupDesc = t.Element("PointIssuePartnerGroupDesc").GetValue<string>()
                                   }).ToList();
            return transactions;
        }
    }


}
