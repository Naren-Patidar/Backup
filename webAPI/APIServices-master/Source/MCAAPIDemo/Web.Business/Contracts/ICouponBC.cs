using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
using PdfSharp.Pdf;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;


namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface ICouponBC
    {
        List<CouponDetails> GetAvailableCoupons(out int totalCoupons, long householdId);
        List<CouponDetails> GetRedeemedCoupons(long householdId, string culture, Dictionary<string, string> resources);
        PdfBackgroundTemplate GetCouponBackgroungTemplate(string title, string culture);
        bool RecordPrintAtHomeDetails(List<CouponDetails> couponList, string customerId, AccountDetails customerAccountDetails);
    }
}
