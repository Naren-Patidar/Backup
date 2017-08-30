using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Data;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Vouchers
{
    public class VoucherUsageSummary : ComparableEntity<VoucherUsageSummary>
    {
        public string HouseholdId { get; set; }
        public string PeriodName { get; set; }
        public Decimal Value { get; set; }
        public string WhenUsed { get; set; }
        public string WhereUsed { get; set; }
        public string AdditionalInfo { get; set; }

        internal override bool AreInstancesEqual(VoucherUsageSummary target)
        {
            return ((target.HouseholdId == this.HouseholdId) &&
                    (target.PeriodName == this.PeriodName) &&
                    (target.Value == this.Value) &&
                    (target.WhenUsed == this.WhenUsed) &&
                    (target.WhereUsed == this.WhereUsed) &&
                    (target.AdditionalInfo == this.AdditionalInfo));
        }

    }

    public class VoucherUsageSummaryLst : BaseEntity<VoucherUsageSummaryLst>
    {
        List<VoucherUsageSummary> _voucherUsageSummaryLstInstance = new List<VoucherUsageSummary>();
        public List<VoucherUsageSummary> VoucherUsageSummaryLstInstance
        {
            get { return _voucherUsageSummaryLstInstance; }
            set { _voucherUsageSummaryLstInstance = value; }
        }

        public override void ConvertFromDataset(DataSet ds)
        {
            DateTime dtTemp = DateTime.Now;
            _voucherUsageSummaryLstInstance = ds.Tables["Table"].AsEnumerable()
            .Select(dr => new VoucherUsageSummary
                                               {
                                                   HouseholdId = dr.GetValue<string>(VoucherUsageSummaryEnum.HouseholdId),
                                                   PeriodName = dr.GetValue<string>(VoucherUsageSummaryEnum.PeriodName),
                                                   Value = dr.GetValue<decimal>(VoucherUsageSummaryEnum.Value),
                                                   WhenUsed = dr.GetValue<string>(VoucherUsageSummaryEnum.WhenUsed).TryParseDate(out dtTemp) ? dtTemp.ToString("o") : String.Empty,
                                                   WhereUsed = dr.GetValue<string>(VoucherUsageSummaryEnum.WhereUsed),
                                                   AdditionalInfo = dr.GetValue<string>(VoucherUsageSummaryEnum.AdditionalInfo)
                                               }).ToList();
        }

        public override void ConvertFromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);
            _voucherUsageSummaryLstInstance = (from t in xDoc.Descendants("Table")
                                               select new VoucherUsageSummary
                                               {
                                                   HouseholdId = t.Element(VoucherUsageSummaryEnum.HouseholdId).GetValue<string>(),
                                                   PeriodName = t.Element(VoucherUsageSummaryEnum.PeriodName).GetValue<string>(),
                                                   Value = t.Element(VoucherUsageSummaryEnum.Value).GetValue<decimal>(),
                                                   WhenUsed = t.Element(VoucherUsageSummaryEnum.WhenUsed).GetValue<string>(),
                                                   WhereUsed = t.Element(VoucherUsageSummaryEnum.WhereUsed).GetValue<string>(),
                                                   AdditionalInfo = t.Element(VoucherUsageSummaryEnum.AdditionalInfo).GetValue<string>()
                                               }).ToList();
        }
    }
}