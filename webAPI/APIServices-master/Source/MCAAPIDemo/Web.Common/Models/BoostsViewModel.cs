using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class BoostsViewModel
    {
        RewardAndToken _rewardAndToken = new RewardAndToken();
        public bool isCurrenltyBCVEPeriod { get; set; }

        public RewardAndToken rewardAndToken
        {
            get { return _rewardAndToken; }
            set { _rewardAndToken = value; }
        }
    }
}
