using System;
using System.Collections.Generic;
using System.Web.Hosting;

using Tesco.Marketing.IT.ClubcardCoupon.DataContract;
using Tesco.Marketing.IT.ClubcardCoupon.AdHocCouponService;

namespace Tesco.ClubcardCoupon.AdHocAutoStart
{
    public class AdHocServiceAutoStart : IProcessHostPreloadClient
    {
        public void Preload(string[] parameters)
        {
            try
            {
                ClubcardAdHocCouponService obj = new ClubcardAdHocCouponService();
                AdHocCouponRequest aaa = new AdHocCouponRequest();
                aaa.MailingNumber = "IIS";
                aaa.TriggerNumber = 100;
                obj.IssueAdHocCoupons(aaa);
            }
            catch { }
        }
    }
}
