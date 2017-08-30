using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers
{
    [Serializable]
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
        List<VoucherRewardDetails> _voucherRewardDetails;
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
                                            RewardIssued = t.Element(VoucherRewardDetailsEnum.Reward_Issued.ToString()).GetValue<decimal>(),
                                            RewardUsed = t.Element(VoucherRewardDetailsEnum.Reward_Used.ToString()).GetValue<decimal>(),
                                            RewardLeftOver = t.Element(VoucherRewardDetailsEnum.Reward_Left_Over.ToString()).GetValue<decimal>()
                                        }).ToList();
        }
    }
}
