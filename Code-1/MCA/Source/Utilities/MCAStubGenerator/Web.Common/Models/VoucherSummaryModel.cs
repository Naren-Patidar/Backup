using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    [Serializable]
    public class VoucherSummaryModel
    {
        VoucherRewardDetailsOverallSummaryModel _voucherRewardDetailsOverallSummaryModel = null;
        VoucherRewardsMilesModel _voucherRewardsMilesModel = null;
        private bool _showHoldingPage = false;

        public VoucherSummaryModel(VoucherRewardDetailsOverallSummaryModel voucherRewardOverallSummary,
                                    VoucherRewardsMilesModel rewardsMiles)
        {
            this._voucherRewardsMilesModel = rewardsMiles;
            this._voucherRewardDetailsOverallSummaryModel = voucherRewardOverallSummary;

            this._showHoldingPage = (this.VoucherRewardDetailsOverallSummaryModel == null ||
                                    (this.VoucherRewardDetailsOverallSummaryModel.TotalRewardIssued == 0 &&
                                    this.VoucherRewardDetailsOverallSummaryModel.TotalRewardLeftOver == 0 &&
                                    this.VoucherRewardDetailsOverallSummaryModel.TotalVoucherIssuedLastTwoYears == 0));
        }

        public VoucherRewardDetailsOverallSummaryModel VoucherRewardDetailsOverallSummaryModel
        {
            get
            {
                return this._voucherRewardDetailsOverallSummaryModel;
            }
        }

        public VoucherRewardsMilesModel VoucherRewardsMilesModel { get { return this._voucherRewardsMilesModel; } }

        public bool isDotcomCustomerIDEmpty
        {
            get { return true; }
        }

        public bool ShowHoldingPage
        {
            get
            {
                return this._showHoldingPage;
            }
        }
    }
}
