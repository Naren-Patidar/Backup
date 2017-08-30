using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// ClubcardCoupon CheckoutResponse Entity
    /// </summary>
    [DataContract]
    public class CheckOutResponse : ResponseBase
    {
        public CheckOutResponse()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Overloaded Constructer for Overloading the members of Base Class
        /// </summary>
        /// <param name="errorLogID">Represents errorLogID</param>
        /// <param name="errorStatusCode">Represents errorStatusCode</param>
        /// <param name="errorMessage">Represents errorMessage</param>
        public CheckOutResponse(string errorLogID, string errorStatusCode, string errorMessage)
            : base(errorLogID, errorStatusCode, errorMessage)
        {
        }

        /// <summary>
        /// Represents the SessionId of the transaction
        /// </summary>
        [DataMember]
        public string SessionId { get; set; }

        /// <summary>
        /// Represents the 12 digit SmartAlphanumericCode of a Coupon
        /// </summary>
        /// <example>GHCKXFLJ234C</example>
        [DataMember]
        public string SmartAlphaNumericCode { get; set; }

        /// <summary>
        /// Represents the 22 digit SmartBarcode of a Coupon
        /// </summary>
        /// <example>9912348765498097654567</example>
        [DataMember]
        public string SmartBarcodeNumber { get; set; }

        /// <summary>
        /// Represents the redemption status of the coupon. Returns 15 if the coupon is successfully redeemed
        /// </summary>
        /// <example>Redeemed/Unredeemed</example>
        [DataMember]
        public Int16 RedemptionStatus
        { get; set; }        
    }
}