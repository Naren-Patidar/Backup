using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using System.Linq;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using System.Globalization;
using Newtonsoft.Json;
namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.ChristmasSaver
{
    public class ChristmasSaverSummary : ComparableEntity<ChristmasSaverSummary>
    {
        public string TransactionDateTime { get; set; }
        public string AmountSpent { get; set; }

        internal override bool AreInstancesEqual(ChristmasSaverSummary target)
        {
            DateTime dtTarget, dtSource;

            return (target.TransactionDateTime.TryParseDate(out dtTarget) 
                    && this.TransactionDateTime.TryParseDate(out dtSource) 
                    && dtTarget.CompareDateTimeOnly(dtSource)
                    && target.AmountSpent == this.AmountSpent);
        }
    }

    public class ChristmasSaverSummaryList : BaseEntity<ChristmasSaverSummaryList>
    {
        public ChristmasSaverSummaryList()
        {
            this._christmasSaverSummary = new List<ChristmasSaverSummary>();
        }

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
                    TransactionDateTime = t.Element(ChristmasSaverSummaryEnum.TransactionDateTime).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString("o") : String.Empty,
                    AmountSpent = t.Element(ChristmasSaverSummaryEnum.AmountSpent).GetValue<string>().TryParseDecimal().ToString()
                }).ToList();
        }
    }
}
