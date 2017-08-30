using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class CouponViewModel
    {
        Int64 householdID = 0;
        Int32 avalableCouponCount = 0;
        Int32 issuedCouponCount = 0;
        Int32 redeemedCouponCount = 0;
        List<CouponDetails> availableCoupons = new List<CouponDetails>();
        List<CouponDetails> redeemedCoupons = new List<CouponDetails>();

        
        /// <summary>
        /// Property to get or set the unused / available coupons count
        /// </summary>
        public Int32 AvalableCouponCount
        {
            get { return avalableCouponCount; }
            set { avalableCouponCount = value; }
        }

        /// <summary>
        /// Property to get or set the issued coupons count
        /// </summary>
        public Int32 IssuedCouponCount
        {
            get { return issuedCouponCount; }
            set { issuedCouponCount = value; }
        }

        /// <summary>
        /// Property to get or set the used / redeemed coupons count
        /// </summary>
        public Int32 RedeemedCouponCount
        {
            get { return redeemedCouponCount; }
            set { redeemedCouponCount = value; }
        }

        /// <summary>
        /// Property to get or set available coupons
        /// </summary>
        public List<CouponDetails> AvailableCoupons
        {
            get { return availableCoupons; }
            set { availableCoupons = value; }
        }

        /// <summary>
        /// Property to get or set the redeemed coupons
        /// </summary>
        public List<CouponDetails> RedeemedCoupons
        {
            get { return redeemedCoupons; }
            set { redeemedCoupons = value; }
        }


        public Int64 HouseholdID
        {
            get { return householdID; }
            set { householdID = value; }
        }
    }
}
