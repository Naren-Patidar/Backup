using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Collections;
using PdfSharp.Pdf;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common; 

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts
{
    public interface IGeneratePDFForMLS
    {
        PdfDocument GetCouponsAndVouchersDocument(List<VoucherDetails> voucherdetailsList, List<CouponDetails> couponDetailsList, AccountDetails customerAccountDetails, CouponBackgroundTemplate resouceValues);
    }
}
