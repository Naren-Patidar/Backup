using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// ClubcardCoupon CouponInformationResponse Inheriting ResponseBase for Error details
    /// </summary>
    public class CouponInformationResponse : ResponseBase
    {
        public CouponInformationResponse()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Overloaded Constructer for Overloading the members of Base Class
        /// </summary>
        /// <param name="errorLogID">Represents errorLogID</param>
        /// <param name="errorStatusCode">Represents errorStatusCode</param>
        /// <param name="errorMessage">Represents errorMessage</param>
        public CouponInformationResponse(string errorLogID, string errorStatusCode, string errorMessage)
            : base(errorLogID, errorStatusCode, errorMessage)
        {
        }

        /// <summary>
        /// Returns details of a Coupon
        /// </summary>
        /// <remarks>Returns couponinformation of a coupon</remarks>
        /// <example>Coupon Information includes: SmartBarcode, SmartAlphanumericCode, CouponStatusId, IssuanceDate, IssuanceStore, CouponIssuanceChannel, RedemptionUtilized, CouponInstanceId, Listof RedemptionInfo</example>
        [DataMember]
        public CouponInformation CouponDetails { get; set; }
    }
}