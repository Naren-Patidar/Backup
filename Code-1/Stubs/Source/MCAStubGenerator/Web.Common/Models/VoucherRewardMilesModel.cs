using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    [Serializable]
    public class VoucherRewardsMilesModel : ComparableEntity<VoucherRewardsMilesModel>
    {
        public string optedForMiles { get; set; }
        public string milesRate { get; set; }
        public string totalRewardPoints { get; set; }
        public string summaryFormattedVoucherValue { get; set; }

        internal override bool AreInstancesEqual(VoucherRewardsMilesModel target)
        {
            return 
            (
                optedForMiles.Equals(target.optedForMiles)
                && milesRate.Equals(target.milesRate)
                && totalRewardPoints.Equals(target.totalRewardPoints)
                && summaryFormattedVoucherValue.Equals(target.summaryFormattedVoucherValue)
            );
        }
    }    
}