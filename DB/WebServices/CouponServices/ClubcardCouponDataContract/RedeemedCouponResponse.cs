using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// ClubcardCoupon RedeemedCouponResponse inheriting ResponseBase for error details
    /// </summary>
    public class RedeemedCouponResponse : ResponseBase
    {
        public RedeemedCouponResponse()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Overloaded Constructer for Overloading the members of Base Class
        /// </summary>
        /// <param name="errorLogID">Represents errorLogID</param>
        /// <param name="errorStatusCode">Represents errorStatusCode</param>
        /// <param name="errorMessage">Represents errorMessage</param>
        public RedeemedCouponResponse(string errorLogID, string errorStatusCode, string errorMessage)
            : base(errorLogID, errorStatusCode, errorMessage)
        {
        }

        /// <summary>
        /// Represents HouseholdId of a Customer
        /// </summary>
        /// <example>Any Positive integer</example>
        [DataMember]
        public long? RequestedHouseHoldId { get; set; }

        /// <summary>
        /// Represents the total number of Redeemed coupons in the specified redemption length
        /// </summary>
        /// <example>Any Positive integer</example>
        [DataMember]
        public int? TotalRedeemCoupon { get; set; }

        /// <summary>
        /// Returns the list of Coupon Information redeemed in the specified redemption length
        /// </summary>
        /// <example>Coupon Information includes: SmartBarcode, SmartAlphanumericCode, CouponStatusId, IssuanceDate, IssuanceStore, CouponIssuanceChannel, RedemptionUtilized, CouponInstanceId, List of RedemptionInfo</example>
        [DataMember]
        public List<CouponInformation> CouponList { get; set; }
    }
}