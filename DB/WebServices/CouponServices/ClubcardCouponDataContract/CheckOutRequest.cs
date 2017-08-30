using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// ClubcardCoupon CheckoutRequest Entity
    /// </summary>
    [DataContract]
    public class CheckOutRequest
    {
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
        /// Represents the ClubcardNumber number of a Customer
        /// </summary>
        /// <example>63465780954678934</example>
        [DataMember]
        public string ClubcardNumber { get; set; }

        /// <summary>
        /// Represents the channel through which customer is redeeming a Coupon
        /// </summary>
        /// <example>GHS, Store, Other</example>
        [DataMember]
        public CouponCheckOutChannel RedemptionChannel { get; set; }
        
        /// <summary>
        /// Represents the Storenumber through which a coupon Remption or Issuance is done
        /// </summary>
        [DataMember]
        public Int16 StoreNumber { get; set; }

        /// <summary>
        /// Represents the TillId through which a coupon is Redeemed
        /// </summary>
        [DataMember]
        public Int16 TillId { get; set; }

        /// <summary>
        /// Represents the Type of Till where a coupon is Redeemed
        /// </summary>
        [DataMember]
        public Int16 TillType { get; set; }

        /// <summary>
        /// Represents Timestamp of when a coupon is Redeemed
        /// </summary>
        [DataMember]
        public DateTime TxnTimeStamp { get; set; }

        /// <summary>
        /// Returns true if the coupon redemption happens Offline
        /// </summary>
        [DataMember]
        public bool IsOfflineTransaction { get; set; }

        /// <summary>
        /// Represents CashierNumber who collects the cash at Store
        /// </summary>
        [DataMember]
        public string CashierNumber { get; set; }

        /// <summary>
        /// Returns true if the redeemed coupon has to be unredeemed back and make it active
        /// </summary>
        [DataMember]
        public bool IsReversal { get; set; }
    }
}