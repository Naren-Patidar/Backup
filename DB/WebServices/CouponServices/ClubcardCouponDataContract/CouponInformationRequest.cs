using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// ClubcardCoupon CouponInformationRequest Entity
    /// </summary>
    [DataContract]
    public class CouponInformationRequest
    {
        /// <summary>
        /// Represents the 22 digit SmartBarcode of a Coupon
        /// </summary>
        /// <example>9912348765498097654567</example>
        [DataMember]
        public string SmartBarcode { get; set; }

        /// <summary>
        /// Represents the 12 digit SmartAlphanumericCode of a Coupon
        /// </summary>
        /// <example>GHCKXFLJ234C</example>
        [DataMember]
        public string SmartAlphaCode { get; set; }

        ///// <summary>
        ///// Represents HouseholdId of a Customer
        ///// </summary>
        ///// <example>45608</example>
        //[DataMember]
        //public long? HouseholdId { get; set; }

        ///// <summary>
        ///// Represents the ClubcardNumber number of a Customer
        ///// </summary>
        ///// <example>63465780954678934</example>
        //[DataMember]
        //public long? ClubcardNumber { get; set; }

        /// <summary>
        /// Returns true if the image is Required
        /// </summary>
        /// <example>True/False</example>
        [DataMember]
        public bool ImageRequired { get; set; }
    }
}