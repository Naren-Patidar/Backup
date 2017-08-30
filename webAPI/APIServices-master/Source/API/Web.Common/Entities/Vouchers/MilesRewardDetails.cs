using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Data;


namespace Tesco.ClubcardProducts.MCA.API.Common.Entities
{
    public class MilesRewardDetails : BaseEntity<MilesRewardDetails>
    {
        public string HouseholdId { get; set; }
        public string PeriodName { get; set; }
        public int RewardPoints { get; set; }
        public decimal RewardIssued { get; set; }
        public decimal RewardUsed { get; set; }
        public string ReasonCode { get; set; }
        public decimal RewardLeftOver { get; set; }
        public string ColPeriodNo { get; set; }
    }

    public class MilesRewardDetailsLst : BaseEntity<MilesRewardDetailsLst>
    {
        List<MilesRewardDetails> _milesRewardDetailsLstInstance = new List<MilesRewardDetails>();
        public List<MilesRewardDetails> MilesRewardDetailsLstInstance
        {
            get { return _milesRewardDetailsLstInstance; }
            set { _milesRewardDetailsLstInstance = value; }
        }

        public override void ConvertFromDataset(DataSet ds)
        {
            XDocument xDoc = XDocument.Parse(ds.GetXml());
            this.ConvertFromXml(xDoc.ToString());
        }

        public override void ConvertFromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);
            _milesRewardDetailsLstInstance = (from t in xDoc.Descendants("Table")
                                              select new MilesRewardDetails
                                              {
                                                  HouseholdId = t.Element(MilesRewardDetailsEnum.Household_ID).GetValue<string>(),
                                                  PeriodName = t.Element(MilesRewardDetailsEnum.Period_Name).GetValue<string>(),
                                                  RewardPoints = (int)t.Element(MilesRewardDetailsEnum.Reward_Points).GetValue<int>(),
                                                  RewardIssued = (decimal)t.Element(MilesRewardDetailsEnum.Reward_Issued).GetValue<decimal>(),
                                                  RewardUsed = (decimal)t.Element(MilesRewardDetailsEnum.Reward_Used).GetValue<decimal>(),
                                                  RewardLeftOver = (decimal)t.Element(MilesRewardDetailsEnum.Reward_Left_Over).GetValue<decimal>(),
                                                  ReasonCode = t.Element(MilesRewardDetailsEnum.Reason_Code).GetValue<string>(),
                                                  ColPeriodNo = t.Element(MilesRewardDetailsEnum.ColPeriodNo).GetValue<string>()
                                              }).ToList();
        }
    }
}
