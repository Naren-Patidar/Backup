using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.ChristmasSaver;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class VoucherAndXmasSaverModel
    {
        List<ChristmasSaverSummary> _christmasSaverSummary = null;
        List<VoucherUsageSummary> _voucherUsageSummary = null;
        bool _isXmasMember = false;

        public VoucherAndXmasSaverModel(List<VoucherUsageSummary> vouchers, List<ChristmasSaverSummary> christmasSaverSummary, bool isXmasMember)
        {
            this._voucherUsageSummary = vouchers;
            this._christmasSaverSummary = christmasSaverSummary;
            this._isXmasMember = isXmasMember;
        }

        public List<VoucherUsageSummary> voucherUsageSummary
        {
            get { return _voucherUsageSummary; }           
        }

        public List<ChristmasSaverSummary> voucherChristmasSaverSummaryModel
        {
            get { return _christmasSaverSummary; }
        }

        public bool IsXmasMember { get { return this._isXmasMember; } }
    }
}
