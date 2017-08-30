using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using System;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Vouchers
{
    public class VoucherRewardDetails : ComparableEntity<VoucherRewardDetails>
    {
        public decimal RewardIssued { get; set; }
        public decimal RewardUsed { get; set; }
        public decimal RewardLeftOver { get; set; }

        internal override bool AreInstancesEqual(VoucherRewardDetails target)
        {
            return (target.RewardIssued == this.RewardIssued) &&
                    (target.RewardUsed == this.RewardUsed) &&
                    (target.RewardLeftOver == this.RewardLeftOver);
        }
    }

    public class VoucherRewardDetailsList : BaseEntity<VoucherRewardDetailsList>
    {
        public VoucherRewardDetailsList()
        {
            this._voucherRewardDetails = new List<VoucherRewardDetails>();
        }

        List<VoucherRewardDetails> _voucherRewardDetails = null;

        public List<VoucherRewardDetails> VoucherRewardDetailList
        {
            get { return _voucherRewardDetails; }
        }

        public override void ConvertFromDataset(DataSet ds)
        {
            XDocument xDoc = XDocument.Parse(ds.GetXml());
            this.ConvertFromXml(xDoc.ToString());
        }

        public override void ConvertFromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);
            _voucherRewardDetails = (from t in xDoc.Descendants("Table")
                                     select new VoucherRewardDetails
                                        {
                                            RewardIssued = t.Element(VoucherRewardDetailsEnum.RewardIssued).GetValue<decimal>(),
                                            RewardUsed = t.Element(VoucherRewardDetailsEnum.RewardUsed).GetValue<decimal>(),
                                            RewardLeftOver = t.Element(VoucherRewardDetailsEnum.RewardLeftOver).GetValue<decimal>()
                                        }).ToList();
        }
    }
}
