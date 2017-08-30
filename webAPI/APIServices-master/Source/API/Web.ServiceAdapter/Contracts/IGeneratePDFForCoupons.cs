using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PdfSharp.Pdf;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts
{
    public  interface IGeneratePDFForCoupons
    {
        PdfDocument GetCouponsDocument(List<CouponDetails> couponDetailsList, AccountDetails customerAccountDetails,CouponBackgroundTemplate template);
    }
}
