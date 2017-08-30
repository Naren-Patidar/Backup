using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// ClubcardCoupon AvailableCouponResponse Entity
    /// </summary>
    [DataContract(Name = "AvailableCoupons")]
    public class AvailableCouponResponse : ResponseBase
    {
        public AvailableCouponResponse()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Overloaded Constructer for Overloading the members of Base Class
        /// </summary>
        /// <param name="errorLogID">Represents errorLogID</param>
        /// <param name="errorStatusCode">Represents errorStatusCode</param>
        /// <param name="errorMessage">Represents errorMessage</param>
        public AvailableCouponResponse(string errorLogID, string errorStatusCode, string errorMessage)
            : base(errorLogID, errorStatusCode, errorMessage)
        {
        }

        /// <summary>
        /// Represents the HouseholdID of a Customer
        /// </summary>
        [DataMember]
        public long? RequestedHouseHoldId { get; set; }

        /// <summary>
        /// Represents the total number of available coupons for the requested HouseholdID
        /// </summary>
        [DataMember]
        public int? TotalCoupon { get; set; }

        /// <summary>
        /// Represents the total number of Active coupons for the requested HouseholdID
        /// </summary>
        [DataMember]
        public int? ActiveCoupon { get; set; }

        /// <summary>
        /// Returns list of available coupons with all Coupon Information for the requested HouseholdID
        /// </summary>
        [DataMember]
        public List<CouponInformation> CouponList { get; set; }
    }
}