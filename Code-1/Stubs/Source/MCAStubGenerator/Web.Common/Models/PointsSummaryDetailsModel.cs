using System;
using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Points;
using System.Globalization;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class PointsSummaryDetailsModel : PointsSummary
    {
        public PointsSummaryDetailsModel(PointsSummary pointsSummary)
        {
            this.PointsSummary = pointsSummary;
        }

        private StatementTypesEnum _statementType;
        public StatementTypesEnum StatementType
        {
            get
            {
                return (StatementTypesEnum)Enum.Parse(typeof(StatementTypesEnum), this.PointsSummary.PointSummaryDescEnglish);
            }
            set
            {
                this._statementType = StatementType;
            }
        }
        public string StartDateTime
        {
            get;
            set;
        }
        public string EndDateTime { get; set; }

        public string TescoPointsChangeFromRewards
        {
            get
            {
                return this.PointsSummary.pointsBreakup[PointsTypeEnum.TescoPointsChangeFromRewards].TryParse<String>(); ;
            }
        }
        public string TescoCarriedForwardPoints
        {
            get
            {
                return this.PointsSummary.pointsBreakup[PointsTypeEnum.TescoCarriedForwardPoints].TryParse<String>();
            }
        }
        public string TescoBankPoints
        {
            get
            {
                return this.TescoBankTotalPoints;
            }
        }
        public string TescoBankBroughtForwardPoints
        {
            get
            {
                return this.PointsSummary.pointsBreakup[PointsTypeEnum.TescoBankBroughtForwardPoints].TryParse<String>();
            }
        }
        public string TescoBankCarriedForwardPoints
        {
            get
            {
                return this.PointsSummary.pointsBreakup[PointsTypeEnum.TescoBankCarriedForwardPoints].TryParse<String>();
            }
        }
        public string TotalCarriedForwardPoints
        {
            get
            {
                return this.PointsSummary.pointsBreakup[PointsTypeEnum.TotalCarriedForwardPoints].TryParse<String>();
            }
        }
        public string TescoPoints
        {
            get
            {
                return this.TescoTotalPoints;
            }
        }
        public string TescoBroughtForwardPoints
        {
            get
            {
                return this.PointsSummary.pointsBreakup[PointsTypeEnum.TescoBroughtForwardPoints].TryParse<String>();
            }
        }
        public string TescoBankPointsChangeFromRewards
        {
            get
            {
                return this.PointsSummary.pointsBreakup[PointsTypeEnum.TescoPointsChangeFromRewards].TryParse<String>();
            }
        }
        public string TescoTotalPoints
        {
            get
            {
                return (this.PointsSummary.pointsBreakup[PointsTypeEnum.TescoPointsChangeFromRewards]
                    + this.PointsSummary.pointsBreakup[PointsTypeEnum.TescoBroughtForwardPoints]
                    + this.PointsSummary.pointsBreakup[PointsTypeEnum.TescoPoints]).TryParse<String>();
            }
        }
        public string TescoBankTotalPoints
        {
            get
            {
                return (this.PointsSummary.pointsBreakup[PointsTypeEnum.TescoBankPoints].TryParse<Int32>()
                + this.PointsSummary.pointsBreakup[PointsTypeEnum.TescoBankBroughtForwardPoints].TryParse<Int32>()).TryParse<String>();
            }
        }
        public DateTime EndDate
        {
            get
            {
                DateTime offerEndDateTime;
                DateTime.TryParse(this.PointsSummary.EndDateTime, out offerEndDateTime);
                return offerEndDateTime;
            }
        }

        public string PtsConvertedToMiles
        {
            get
            {
                int iPtsMiles;
                Int32.TryParse(((this.PointsSummary.TotalReward).TryParse<Decimal>() * 100).TryParse<String>(), NumberStyles.Number,
        CultureInfo.CurrentCulture.NumberFormat, out iPtsMiles);
                return iPtsMiles.TryParse<String>();
            }
        }
        public string MilesAwarded
        {
            get
            {
                return this.PointsSummary.TotalRewardMiles;
            }
        }

        public string TescoTotalRewardXmas { get {
            decimal totalRewards, topUpVouchers, bonusVouchers;
            if (this.PointsSummary.TotalReward.Equals(string.Empty))
                totalRewards = 0;
            else
                Decimal.TryParse(this.PointsSummary.TotalReward, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out totalRewards);

            if (this.PointsSummary.TopUpVouchers.Equals(string.Empty))
                topUpVouchers = 0;
            else
                Decimal.TryParse(this.PointsSummary.TopUpVouchers, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out topUpVouchers);

            if (this.PointsSummary.BonusVouchers.Equals(string.Empty))
                bonusVouchers = 0;
            else
                Decimal.TryParse(this.PointsSummary.BonusVouchers, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out bonusVouchers);

           return (totalRewards + topUpVouchers + bonusVouchers).ToString();
        }
        }
        public string TescoTotalRewards
        {
            get
            {
                return this.PointsSummary.rewardsBreakup[RewardsTypeEnum.TescoTotalReward].TryParse<String>();
            }
        }
        public string TescoBankTotalRewards
        {
            get
            {
                return this.PointsSummary.rewardsBreakup[RewardsTypeEnum.TescoBankTotalReward].TryParse<String>();
            }
        }
        public string TescoRewardMiles
        {
            get
            {
                return this.PointsSummary.rewardsBreakup[RewardsTypeEnum.TescoRewardMiles].TryParse<String>();
            }
        }
        public string TescoBankRewardMiles
        {
            get
            {
                return this.PointsSummary.rewardsBreakup[RewardsTypeEnum.TescoBankRewardMiles].TryParse<String>();
            }
        }
        public string TotalCCVoucher
        {
            get
            {
                decimal totalRewards;
                if (this.PointsSummary.TotalReward.Equals(string.Empty))
                    totalRewards = 0;
                else
                    Decimal.TryParse(this.PointsSummary.TotalReward, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out totalRewards);
                return totalRewards.TryParse<String>();
            }
        }
        public string TopUpVouchers
        {
            get
            {
                decimal topUpVouchers;
                if (this.PointsSummary.TopUpVouchers.Equals(string.Empty))
                    topUpVouchers = 0;
                else
                    Decimal.TryParse(this.PointsSummary.TopUpVouchers, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out topUpVouchers);
                return topUpVouchers.TryParse<String>();
            }
        }
        public string BonusVouchers
        {
            get
            {
                decimal bonusVouchers;
                if (this.PointsSummary.BonusVouchers.Equals(string.Empty))
                    bonusVouchers = 0;
                else
                    Decimal.TryParse(this.PointsSummary.BonusVouchers, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out bonusVouchers);
                return bonusVouchers.TryParse<String>();
            }
        }
        public int OfferID { get; set; }
        public PointsSummary PointsSummary { get; set; }
    }
}
