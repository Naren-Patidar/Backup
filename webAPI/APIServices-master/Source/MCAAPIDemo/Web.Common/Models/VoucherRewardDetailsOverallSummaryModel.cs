using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    [Serializable]
    public class VoucherRewardDetailsOverallSummaryModel : ComparableEntity<VoucherRewardDetailsOverallSummaryModel>
    {
        private List<VoucherRewardDetails> _lstVoucherRewardDetails { get; set; }

        public VoucherRewardDetailsOverallSummaryModel(List<VoucherRewardDetails> lstVoucherRewardDetails)
        {
            _lstVoucherRewardDetails = lstVoucherRewardDetails;

            this._totalVoucherIssuedLastTwoYears = 0;
            this._totalRewardIssued = 0;
            this._totalRewardUsed = 0;
            this._totalRewardLeftOver = 0;

            _lstVoucherRewardDetails.ForEach(x => this._totalRewardIssued += x.RewardIssued); 
            _lstVoucherRewardDetails.ForEach(x => this._totalRewardLeftOver += x.RewardLeftOver);
            _lstVoucherRewardDetails.ForEach(x => this._totalRewardUsed += x.RewardUsed);
            _lstVoucherRewardDetails.ForEach(x => this._totalVoucherIssuedLastTwoYears += x.RewardIssued);
        }

        private decimal _totalVoucherIssuedLastTwoYears = 0;
        private decimal _totalRewardIssued = 0;
        private decimal _totalRewardUsed = 0;
        private decimal _totalRewardLeftOver = 0;

        public decimal TotalRewardIssued
        {
            get
            {
                return this._totalRewardIssued;
            }
        }
        
        public decimal TotalRewardLeftOver
        {
            get 
            { 
                return _totalRewardLeftOver; 
            }
        }

        public decimal TotalRewardUsed
        {
            get 
            { 
                return _totalRewardUsed;
            }
        }

        public decimal TotalVoucherIssuedLastTwoYears
        {
            get 
            { 
                return this._totalVoucherIssuedLastTwoYears;
            }
        }
      
        internal override bool AreInstancesEqual(VoucherRewardDetailsOverallSummaryModel target)
        {
            return TotalVoucherIssuedLastTwoYears.Equals(target.TotalVoucherIssuedLastTwoYears)
                    && TotalRewardIssued.Equals(target.TotalRewardIssued)
                    && TotalRewardUsed.Equals(target.TotalRewardUsed)
                     && TotalRewardLeftOver.Equals(target.TotalRewardLeftOver)
                     && Enumerable.SequenceEqual<VoucherRewardDetails>(this._lstVoucherRewardDetails, target._lstVoucherRewardDetails);

        }
    }
}
