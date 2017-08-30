using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common
{
    public class Clubcard : BaseEntity<Clubcard>
    {        
        public long ClubCardID { get; set; } 
        public int? ClubcardType { get; set; }
        public string ClubCardTypeDesc { get; set; }
        public DateTime? TransactionDateTime { get; set; }
        public int? TransactionType { get; set; }
        public string ClubcardStatusDescEnglish { get; set; }
        public DateTime CardIssuedDate { get; set; }
        public string StoreName { get; set; }
        public string LastUsed { get; set; }
    }

    public class ClubcardDetailsList : BaseEntity<ClubcardDetailsList>
    {
        List<Clubcard> _list = new List<Clubcard>();

        public List<Clubcard> List
        {
            get { return _list; }
            set { _list = value; }
        }

        public override void ConvertFromXml(string xml)
        {
            DateTime dtTemp = new DateTime();
            XDocument xDoc = XDocument.Parse(xml);
            _list = (from t in xDoc.Descendants("ClubcardDetails")
                     select new Clubcard
                     {
                         ClubCardID = t.Element(ClubcardEnum.ClubCardID).GetValue<Int64>(),
                         ClubcardType = t.Element(ClubcardEnum.ClubcardType).GetValue<Int32>(),
                         ClubCardTypeDesc = t.Element(ClubcardEnum.ClubCardTypeDesc).GetValue<string>(),
                         TransactionDateTime = t.Element(ClubcardEnum.TransactionDateTime).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.MinValue,
                         TransactionType = t.Element(ClubcardEnum.TransactionType).GetValue<Int32>(),
                         ClubcardStatusDescEnglish = t.Element(ClubcardEnum.ClubcardStatusDescEnglish).GetValue<string>(),
                         StoreName = t.Element(ClubcardEnum.StoreName).GetValue<string>(),
                     }).ToList();
        }

    }



}
