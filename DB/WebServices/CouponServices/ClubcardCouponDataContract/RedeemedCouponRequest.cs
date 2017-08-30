using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// ClubcardCoupon RedeemedCouponRequest Entity
    /// </summary>
    [DataContract]    
    public class RedeemedCouponRequest : AvailableCouponRequest
    {
        /// <summary>
        /// Represents how many no of days old redeemed coupons the user is asking to get the Coupon details
        /// </summary>
        /// <example>If RedemptionLength is 180, it means that user is requesting for all the coupons which are redeemed in last 180 days</example>
        [DataMember]
        public int? RedemptionLength { get; set; }
    }
}