using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// ClubcardCoupon AvailableCouponRequest Entity
    /// </summary>
    [DataContract]
    public class AvailableCouponRequest
    {
        /// <summary>
        /// Represents the Household ID of a customer
        /// </summary>
        [DataMember]
        public long? HouseHoldId { get; set; }

        /// <summary>
        /// Represents Clubcard Number of a Customer
        /// </summary>
        [DataMember]
        public long? ClubCardNumber { get; set; }

        /// <summary>
        /// Returns true if the Image is required to be printed on a coupon
        /// </summary>
        [DataMember]
        public bool ImageRequired { get; set; }
    }
}