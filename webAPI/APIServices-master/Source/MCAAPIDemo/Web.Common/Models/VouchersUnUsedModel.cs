using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    [Serializable]
    public class VouchersUnUsedModel : ComparableEntity<VouchersUnUsedModel>
    {
        List<VoucherDetails> _voucherList = new List<VoucherDetails>();
        public decimal totalUnusedVouchers { get; set; }
        public bool error { get; set; }

        public List<VoucherDetails> voucherList
        {
            get { return _voucherList; }
            set { _voucherList = value; }
        }

        public List<VoucherDetails> getSelectedVouchers(List<VoucherDetails> vList)
        {
            // Return a list of voucherdetails of the selected vouchers
            var IDs = (from v in this.voucherList where v.Selected select v).ToList();
            var result = vList.Where(v => IDs.Any(v2 => CryptoUtility.DecryptTripleDES(v2.VNumberEncr).ToLower() == v.VoucherNumber.ToLower()));
            return result.ToList();
        }

        internal override bool AreInstancesEqual(VouchersUnUsedModel target)
        {
            return totalUnusedVouchers.Equals(target.totalUnusedVouchers)
                     && Enumerable.SequenceEqual<VoucherDetails>(this._voucherList, target._voucherList);
        }
    }
}
