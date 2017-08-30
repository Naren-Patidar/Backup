using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System.Linq;
using System.Globalization;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Points
{
    [Serializable]
    public class PointsSummary
    {
        string _Salutation = string.Empty;

        public string CustomerType { get; set; }
        public string TotalPoints { get; set; }
        public string TotalReward { get; set; }
        public string TopUpVouchers { get; set; }
        public string BonusVouchers { get; set; }
        public string TotalRewardMiles { get; set; }
        public string NoCoupons { get; set; }
        public string StatementVideo { get; set; }
        
        public string MainClubcardID { get; set; }
        public string RewardMilesRate { get; set; }
        public string StatementType { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string PointSummaryDescEnglish { get; set; }

        public string Salutation
        {
            get { return _Salutation; }
            set { _Salutation = value; }
        }
      

        public Dictionary<PointsTypeEnum, long> pointsBreakup = new Dictionary<PointsTypeEnum, long>();

        public Dictionary<RewardsTypeEnum, decimal> rewardsBreakup = new Dictionary<RewardsTypeEnum, decimal>();

    }
    public class PointsSummaryList : BaseEntity<PointsSummaryList>
    {
        List<PointsSummary> _pointSummaryList;
        public List<PointsSummary> PointSummaryList
        {
            get { return _pointSummaryList; }
        }

        public override void ConvertFromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);
            _pointSummaryList = (from r in xDoc.Descendants("PointsSummaryRec")
                                 select new PointsSummary
                                 {
                                     CustomerType = r.Element(PointsSummaryEnum.CustomerType).GetValue<string>(),
                                     TotalPoints = r.Element(PointsSummaryEnum.TotalPoints).GetValue<string>(),
                                     TotalReward = r.Element(PointsSummaryEnum.TotalReward).GetValue<string>(),
                                     TopUpVouchers = r.Element(PointsSummaryEnum.TopUpVouchers).GetValue<string>(),
                                     BonusVouchers = r.Element(PointsSummaryEnum.BonusVouchers).GetValue<string>(),
                                     TotalRewardMiles = r.Element(PointsSummaryEnum.TotalRewardMiles).GetValue<string>(),
                                     NoCoupons = r.Element(PointsSummaryEnum.NoCoupons).GetValue<string>(),
                                     StatementVideo = r.Element(PointsSummaryEnum.StatementVideo).GetValue<string>(),
                                     Salutation = r.Element(PointsSummaryEnum.Salutation).GetValue<string>(),
                                     MainClubcardID = r.Element(PointsSummaryEnum.MainClubcardID).GetValue<string>(),
                                     RewardMilesRate = r.Element(PointsSummaryEnum.RewardMilesRate).GetValue<string>(),
                                     StatementType = r.Element(PointsSummaryEnum.StatementType).GetValue<string>(),
                                     StartDateTime = r.Element(PointsSummaryEnum.StartDateTime).GetValue<string>(),
                                     PointSummaryDescEnglish = r.Element(PointsSummaryEnum.PointSummaryDescEnglish).GetValue<string>(),
                                     EndDateTime = r.Element(PointsSummaryEnum.EndDateTime).GetValue<string>()
                                 }).ToList();

            for (int i = 0; i < _pointSummaryList.Count; i++)
            {
                foreach (string pointsTypeLiteral in Enum.GetNames(typeof(PointsTypeEnum)))
                {
                    PointsTypeEnum pointsType = (PointsTypeEnum)Enum.Parse(typeof(PointsTypeEnum), pointsTypeLiteral);
                    long points;
                    foreach (XElement r in xDoc.Descendants("PointsSummaryRec"))
                    {
                        if (r.Element(pointsTypeLiteral) != null)
                        {
                            points = r.Element(pointsTypeLiteral).GetValue<long>();
                            if (_pointSummaryList[i].pointsBreakup.ContainsKey(pointsType))
                            {
                                break;
                            }
                            else
                            {
                                _pointSummaryList[i].pointsBreakup.Add(pointsType, points);
                            }
                        }
                        else
                        {
                            points = r.Element(pointsTypeLiteral).GetValue<long>();
                            if (_pointSummaryList[i].pointsBreakup.ContainsKey(pointsType))
                            {
                                break;
                            }
                            else
                            {
                                _pointSummaryList[i].pointsBreakup.Add(pointsType, points);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < _pointSummaryList.Count; i++)
            {
                foreach (string rewardsTypeLiteral in Enum.GetNames(typeof(RewardsTypeEnum)))
                {
                    decimal points;
                    RewardsTypeEnum rewardsType = (RewardsTypeEnum)Enum.Parse(typeof(RewardsTypeEnum), rewardsTypeLiteral);
                    foreach (XElement r in xDoc.Descendants("PointsSummaryRec"))
                    {
                        if (r.Element(rewardsTypeLiteral) != null)
                        {
                            Decimal.TryParse(r.Element(rewardsTypeLiteral).GetValue<string>(), NumberStyles.AllowDecimalPoint,
                                CultureInfo.InvariantCulture, out points);
                            
                            if (_pointSummaryList[i].rewardsBreakup.ContainsKey(rewardsType))
                            {
                                break;
                            }
                            else
                            {
                                _pointSummaryList[i].rewardsBreakup.Add(rewardsType, points);
                            }
                        }
                        else
                        {
                            Decimal.TryParse(r.Element(rewardsTypeLiteral).GetValue<string>(), NumberStyles.AllowDecimalPoint,
                                CultureInfo.InvariantCulture, out points);
                            if (_pointSummaryList[i].rewardsBreakup.ContainsKey(rewardsType))
                            {
                                break;
                            }
                            else
                            {
                                _pointSummaryList[i].rewardsBreakup.Add(rewardsType, points);
                            }
                        }
                    }
                }
            }
        }
    }
}
