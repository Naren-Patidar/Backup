using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Points;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    [Serializable]
    public class MyLatestStatementViewModel : ComparableEntity<MyLatestStatementViewModel>
    {
        PointsSummary _pointsSummary = new PointsSummary();
        public bool isHoldingPageEnabled { get; set; }

        public DateTime? pointsPrevStartDate { get; set; }
        public DateTime? pointsPrevEndDate { get; set; }
        private int _prevOfferId = 0;
        public string voucherPerMileFifty { get; set; }

        private decimal _dMiles = 0;

        public PointsSummary pointsSummary
        {
            get { return _pointsSummary; }
            set { _pointsSummary = value; }        
        }

        public int prevOfferId
        {
            get { return _prevOfferId; }
            set { _prevOfferId = value; }
        }

        public decimal dMiles
        {
            get { return _dMiles; }
            set { _dMiles = value; }
        }

        internal override bool AreInstancesEqual(MyLatestStatementViewModel target)
        {
            return this.pointsSummary.Equals(target.pointsSummary)
                    && this.isHoldingPageEnabled.Equals(target.isHoldingPageEnabled)
                    && this.pointsPrevStartDate.Equals(target.pointsPrevStartDate)
                    && this.pointsPrevEndDate.Equals(target.pointsPrevEndDate)
                    && this.prevOfferId.Equals(target.prevOfferId)
                    && this.voucherPerMileFifty.Equals(target.voucherPerMileFifty)
                    && this.dMiles.Equals(target.dMiles);
        }
    }
}
