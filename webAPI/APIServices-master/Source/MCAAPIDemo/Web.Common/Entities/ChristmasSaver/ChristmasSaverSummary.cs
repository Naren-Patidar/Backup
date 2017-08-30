using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using System.Linq;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System.Globalization;
namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.ChristmasSaver
{
    [Serializable]
    public class ChristmasSaverSummary : ComparableEntity<ChristmasSaverSummary>
    {
        public DateTime TransactionDateTime { get; set; }
        public string AmountSpent { get; set; }

        internal override bool AreInstancesEqual(ChristmasSaverSummary target)
        {
            return ((target.TransactionDateTime.CompareDateTimeOnly(this.TransactionDateTime)) &&
                    (target.AmountSpent == this.AmountSpent));
        }
    }

    public class ChristmasSaverSummaryList : BaseEntity<ChristmasSaverSummaryList>
    {
        List<ChristmasSaverSummary> _christmasSaverSummary;
        public List<ChristmasSaverSummary> christmasSaverSummaryList
        {
            get { return _christmasSaverSummary; }
        }

        public override void ConvertFromDataset(DataSet ds)
        {
            XDocument xDoc = XDocument.Parse(ds.GetXml());
            this.ConvertFromXml(xDoc.ToString());
        }

        public override void ConvertFromXml(string xml)
        {
            DateTime dtTemp = DateTime.Now;
            XDocument xDoc = XDocument.Parse(xml);
            _christmasSaverSummary = 
                (from t in xDoc.Descendants("ViewIsXmasSaverSummary")
                select new ChristmasSaverSummary
                {
                    TransactionDateTime = t.Element(ChristmasSaverSummaryEnum.TransactionDateTime).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.MinValue,
                    AmountSpent = t.Element(ChristmasSaverSummaryEnum.AmountSpent).GetValue<string>().TryParseDecimal().ToString()
                }).ToList();
        }
    }
}
