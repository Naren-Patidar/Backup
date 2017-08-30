using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.ChristmasSaver;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    [Serializable]
    public class VouchersViewModel
    {
        AccountDetails _accountDetails = null;
        VouchersUnUsedModel _vouchersUnUsedModel = null;
        List<VoucherUsageSummary> _voucherUsageSummary = null;
        VoucherRewardDetailsOverallSummaryModel _voucherRewardDetailsOverallSummaryModel = null;
        VoucherRewardsMilesModel _voucherRewardsMilesModel = null;
        VoucherChristmasSaverSummaryModel _voucherChristmasSaverSummaryModel = null;

        public bool vouchersExpired { get; set; }
        public bool _isDotcomCustomerIDEmpty = true;
        
        public VouchersViewModel()
        {
            this._vouchersUnUsedModel = new VouchersUnUsedModel();
            this._voucherUsageSummary = new List<VoucherUsageSummary>();
            this._voucherRewardsMilesModel = new VoucherRewardsMilesModel();
            this._voucherChristmasSaverSummaryModel = new VoucherChristmasSaverSummaryModel();
        }

        public AccountDetails accountDetails
        {
            get { return _accountDetails; }
            set { _accountDetails = value; }
        }
        
        public VouchersUnUsedModel vouchersUnUsedModel
        {
            get { return _vouchersUnUsedModel; }
            set { _vouchersUnUsedModel = value; }
        }

        public VoucherRewardDetailsOverallSummaryModel voucherRewardDetailsOverallSummaryModel
        {
            get { return _voucherRewardDetailsOverallSummaryModel; }
            set { _voucherRewardDetailsOverallSummaryModel = value; }
        }
        public List<VoucherUsageSummary> voucherUsageSummary
        {
            get { return _voucherUsageSummary; }
            set { _voucherUsageSummary = value; }
        }

        public VoucherRewardsMilesModel voucherRewardsMilesModel
        {
            get { return _voucherRewardsMilesModel; }
            set { _voucherRewardsMilesModel = value; }
        }

        public VoucherChristmasSaverSummaryModel voucherChristmasSaverSummaryModel
        {
            get { return _voucherChristmasSaverSummaryModel; }
            set { _voucherChristmasSaverSummaryModel = value; }
        }

        public bool isDotcomCustomerIDEmpty
        {
            get { return _isDotcomCustomerIDEmpty; }
            set { _isDotcomCustomerIDEmpty = value; }
        }
    }
}
