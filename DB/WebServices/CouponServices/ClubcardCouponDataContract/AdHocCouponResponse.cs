using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// This datacontract represent
    /// AdHoc Coupon response
    /// </summary>
    [DataContract]
    public class AdHocCouponResponse
    {
        /// <summary>
        /// SmartAlphaNumericCode is required if coupoun has to be redeem through GHS mode
        /// </summary>
        [DataMember]
        public string SmartAlphaNumericCode { get; set; }

        /// <summary>
        /// SmartBarcodeNumber is required if coupon has to be redeem from TILL
        /// </summary>
        [DataMember]
        public string SmartBarcodeNumber { get; set; }

        /// <summary>
        /// CouponExpiryDate after this coupon can't be redeemed
        /// </summary>
        [DataMember]
        public DateTime CouponExpiryDate { get; set; }

        /// <summary>
        /// CouponInstanceId: This is Clubcard Coupon system internal tracker value
        /// </summary>
        [DataMember]
        public Int64 CouponInstanceId { get; set; }

        /// <summary>
        /// MaxRedemptionLimit: How many times coupon can be used
        /// </summary>
        [DataMember]
        public Int16 MaxRedemptionLimit { get; set; }

        /// <summary>
        /// CouponErrorCode: For internal use
        /// </summary>
        public Int16 CouponErrorCode { get; set; }

        /// <summary>
        /// CouponErrorMessage: For internal use
        /// </summary>
        public string CouponErrorMessage { get; set; }

        /// <summary>
        /// CouponClassId: For internal use
        /// </summary>
        public Int64 CouponClassId { get; set; }

        /// <summary>
        /// EANBarcode: For internal use
        /// </summary>
        public string EANBarcode { get; set; }

        /// <summary>
        /// AlphaCode: For internal use
        /// </summary>
        public string AlphaCode { get; set; }

        /// <summary>
        /// Redemption Channel for the AdHoc Coupons - For Internal use
        /// </summary>
        public string RedemptionChannel { get; set; }
    }
}
