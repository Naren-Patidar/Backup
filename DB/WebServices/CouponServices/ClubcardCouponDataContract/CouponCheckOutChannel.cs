using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// ClubcardCoupon CheckOutChannel Enumeration
    /// </summary>
    [DataContract]
    public enum CouponCheckOutChannel
    {
        /// <summary>
        /// Enumeration for the redemption channel "Dotcom"
        /// </summary>
        /// <example>The CouponCheckoutChannel for Dotcom is 1</example>
        [EnumMember]
        DotCom = 1,
        
        /// <summary>
        /// Enumeration for the redemption channel "Store"
        /// </summary>
        /// <example>The CouponCheckoutChannel for Store is 2</example>
        [EnumMember]
        Storeline = 2,
        
        /// <summary>
        /// Enumeration for the redemption channel "Other"
        /// </summary>
        /// <example>The CouponCheckoutChannel for Other is 3</example>
        [EnumMember]
        Others = 3
    }
}