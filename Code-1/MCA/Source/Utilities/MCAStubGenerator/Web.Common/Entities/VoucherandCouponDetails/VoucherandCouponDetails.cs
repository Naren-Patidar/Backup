using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.VoucherandCouponDetails
{
   public class VoucherandCouponDetails
    {
        List<VoucherDetails> _voucherDetails = new List<VoucherDetails>();        
        List<CouponDetails> _couponDetails = new List<CouponDetails>();

        public List<VoucherDetails> VoucherDetails
        {
            get { return _voucherDetails; }
            set { _voucherDetails = value; }
        }

        public List<CouponDetails> CouponDetails
        {
            get { return _couponDetails; }
            set { _couponDetails = value; }
        } 

    }
}
