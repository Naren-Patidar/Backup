using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// ClubcardCoupon CouponAtTillRequest Entity
    /// </summary>
    [DataContract]
    public class CouponAtTillRequest
    {
        /// <summary>
        /// Represents the CustomerId/ClubcardAccountId of a Coupon
        /// </summary>
        [DataMember]
        public Int64 CustomerId { get; set; }

        /// <summary>
        /// Represents the Clubcard Number of a Coupon
        /// </summary>
        /// <example>63465780954678934</example>
        [DataMember]
        public Int64 ClubcardNumber { get; set; }

        /// <summary>
        /// Represents Store Number where the Coupon is Issued for Till Coupons
        /// </summary>
        /// <example>Any Positive integer</example>
        [DataMember]
        public Int16 StoreNumber { get; set; }

        /// <summary>
        /// Represents TillId where the Coupon is Issued for Till Coupons
        /// </summary>
        /// <example>Any Positive integer</example>
        [DataMember]
        public Int16 TillId { get; set; }

        /// <summary>
        /// Represents TillBankId where the Coupon is Issued for Till Coupons
        /// </summary>
        /// <example>Any Positive integer</example>
        [DataMember]
        public Int16 TillBankId { get; set; }

        /// <summary>
        /// Represents OperatorId where the Coupon is Issued for Till Coupons
        /// </summary>
        /// <example>Any Positive integer</example>
        [DataMember]
        public Int16 OperatorId { get; set; }
    }


    /// <summary>
    /// ClubcardCoupon CouponAtTill Entity
    /// </summary>
    [DataContract]
    public class CouponAtTill : ResponseBase
    {
        public CouponAtTill()
            : this(null, null, null)
        {
            ListLineInfo = new List<LineInfo>();
        }

        /// <summary>
        /// Overloaded Constructer for Overloading the members of Base Class
        /// </summary>
        /// <param name="errorLogID">Represents errorLogID</param>
        /// <param name="errorStatusCode">Represents errorStatusCode</param>
        /// <param name="errorMessage">Represents errorMessage</param>
        public CouponAtTill(string errorLogID, string errorStatusCode, string errorMessage)
            : base(errorLogID, errorStatusCode, errorMessage)
        {
            ListLineInfo = new List<LineInfo>();
        }

        /// <summary>
        /// Represents CouponClassId of a Coupon Class
        /// </summary>
        /// <example>Any Positive integer</example>
        public Int64 CouponClassId { get; set; }
        
        /// <summary>
        /// Represents CouponInstanceId for a CouponClass
        /// </summary>
        /// <example>Any positive integer</example>
        public Int64 CouponInstanceId { get; set; }

        /// <summary>
        /// Represents CustomerId/ClubcardAccountId of a Customer
        /// </summary>
        /// <example>Any positive integer</example>
        [DataMember]
        public Int64 CustomerId { get; set; }

        /// <summary>
        /// Represents Coupon TemplateId of a Coupon to be printed at Till
        /// </summary>
        /// <example>Any Positive integer</example>
        [DataMember]
        public string CouponTemplateId { get; set; }

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
        public decimal SmartBarCode { get; set; }

        /// <summary>
        /// Returns List of Coupon Lines to be printed at Till
        /// </summary>
        /// <example>List of Coupon lines include LineNumber, LineText, LineFormat</example>
        [DataMember]
        public List<LineInfo> ListLineInfo { get; set; }
    }

    /// <summary>
    /// ClubcardCoupon LineInfo Entity
    /// </summary>
    [DataContract]
    public class LineInfo
    {
        /// <summary>
        /// Represents the LineNumber out of 10 Coupon Lines to be printed at Till
        /// </summary>
        /// <example>LineNumber starts with 1 and ends with 10</example>
        [DataMember]
        public string LineNumber { get; set; }

        /// <summary>
        /// Represents the LineText to be printed at Specified LineNumber
        /// </summary>
        /// <example>10 pounds off when you buy Apple Iphone</example>
        [DataMember]
        public string LineText { get; set; }

        /// <summary>
        /// Represents the LineText Format to be printed at Specified LineNumber
        /// </summary>
        /// <example>Bold, Italic</example>
        [DataMember]
        public string LineFormat { get; set; }
    }
}
