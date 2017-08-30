using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// ClubcardCoupon ListCouponClass Entity
    /// </summary>
    [CollectionDataContract]
    public class ListCouponClass : List<CouponClass>
    {
        ListCouponClass() : base() { }
    }

    /// <summary>
    /// ClubcardCoupon CouponClass Entity
    /// </summary>
    [DataContract]
    public class CouponClass
    {
        /// <summary>
        /// Represents the trigger number of a coupon
        /// </summary>
        /// <remarks>Trigger  Number</remarks>
        /// <example>Any positive integer. E.g: 100</example>
        [DataMember]
        public Int16? TriggerNumber { get; set; }

        /// <summary>
        /// Represents the mailing number of a coupon
        /// </summary>
        /// <remarks>Mailing Number</remarks>
        /// <example>Can be a 3 digit alphanumeric code. E.g: "IT3" The combination of mailing number and trigger number will be always unique</example>
        [DataMember]
        public string StatementNumber { get; set; }

        /// <summary>
        /// Represents the description of a Coupon printed on it
        /// </summary>
        /// <remarks>Coupon Description</remarks>
        /// <example>100 Extra Points when you shop for 30 pounds</example>
        [DataMember]
        public string CouponDescription { get; set; }

        /// <summary>
        /// Represents thumbnail image of a Coupon
        /// </summary>
        /// <remarks>Thumbnail image of a coupon</remarks>
        /// <example>Thumbnail Image</example>
        [DataMember]
        public byte[] CouponImageThumbnail { get; set; }

        /// <summary>
        /// Represents full image of a Coupon
        /// </summary>
        /// <remarks>Full image of a coupon</remarks>
        /// <example>Full Image</example>
        [DataMember]
        public byte[] CouponImageFull { get; set; }

        /// <summary>
        /// Represents thumbnail image file name
        /// </summary>
        /// <remarks>Thumbnail image file name, generally file name will be "MailingNumber_Triggernumber_T" or "MailingNumber_TriggerNumber_F", where F and T represents front and back images</remarks>
        /// <example>IT3_100_T, IT3_100_F</example>
        [DataMember]
        public string ThumbnailImageName { get; set; }

        /// <summary>
        /// Represents full image file name
        /// </summary>
        /// <remarks>Full image file name, generally file name will be "MailingNumber_Triggernumber_T" or "MailingNumber_TriggerNumber_F", where F and T represents front and back images</remarks>
        /// <example>IT3_100_T, IT3_100_F</example>
        [DataMember]
        public string FullImageName { get; set; }

        /// <summary>
        /// Represents end date until which a coupon can be redeemed
        /// </summary>
        /// <remarks>End date until which a customer can redeem a coupon</remarks>
        /// <example>dd/MM/YYYY</example>
        [DataMember]
        public DateTime? RedemptionEndDate { get; set; }

        /// <summary>
        /// Represents Start date of issueing a coupon to a customer
        /// </summary>
        /// <remarks>Start date of which a coupon can be issued to a customer</remarks>
        /// <example>dd/MM/YYYY</example>
        [DataMember]
        public DateTime? IssuanceStartDate { get; set; }

        /// <summary>
        /// Represents Start time of issueing a coupon to a customer
        /// </summary>
        /// <remarks>Start time of which a coupon can be issued to a customer</remarks>
        /// <example>HH:MM:SS</example>
        [DataMember]
        public DateTime? IssuanceStartTime { get; set; }

        /// <summary>
        /// Represents End date of issueing a coupon to a customer
        /// </summary>
        /// <remarks>End date of which a coupon can be issued to a customer</remarks>
        /// <example>dd/MM/YYYY</example>
        [DataMember]
        public DateTime? IssuanceEndDate { get; set; }

        /// <summary>
        /// Represents End time of issueing a coupon to a customer
        /// </summary>
        /// <remarks>End time of which a coupon can be issued to a customer</remarks>
        /// <example>HH:MM:SS</example>
        [DataMember]
        public DateTime? IssuanceEndTime { get; set; }

        /// <summary>
        /// Represents Channel through which a coupon can be issued to a customer
        /// </summary>
        /// <remarks>Channel through which a coupon can be issued to a customer</remarks>
        /// <example>Statement, LCM, TILL</example>
        [DataMember]
        public string IssuanceChannel { get; set; }

        /// <summary>
        /// Channel through which customer can redeem a coupon
        /// </summary>
        /// <remarks>Channel through which a customer can redeem a coupon. Ideally it will be Store or GHS or Other</remarks>
        /// <example>GHS, Store, Other</example>
        [DataMember]
        public string RedemptionChannel { get; set; }

        /// <summary>
        /// Maxumum number of times a coupon can be redeemed
        /// </summary>
        /// <remarks>The maximum no of times a coupon can be redeemed</remarks>
        /// <example>Any positive integer, E.G: 3</example>
        [DataMember]
        public Int16? MaxRedemptionLimit { get; set; }

        /// <summary>
        /// Represents the 12 digit SmartAlphanumericCode of a Coupon
        /// </summary>
        /// <remarks>Smart Alphanumeric Code</remarks>
        /// <example>GHCKXFLJ234C</example>
        [DataMember]
        public string AlphaCode { get; set; }

        /// <summary>
        /// Represents the 22 digit SmartBarcode of a Coupon
        /// </summary>
        /// <remarks>Smart Barcode</remarks>
        /// <example>9912348765498097654567</example>
        [DataMember]
        public string EANBarcode { get; set; }

        /// <summary>
        /// Flag whether to generate smartbarcode or not
        /// </summary>
        /// <remarks>Returns true if system needs to generate smart barcodes else returns false</remarks>
        /// <example>True/False</example>
        [DataMember]
        public bool IsGenerateSmartCodes { get; set; }

        /// <summary>
        /// Template used to print the coupon at Tills
        /// </summary>
        /// <remarks>Template Id of a Till Coupon which specifies the design of a coupon printing at Till</remarks>
        /// <example>Any positive integer, E.g: 15</example>
        [DataMember]
        public string TillCouponTemplateNumber { get; set; }

        /// <summary>
        /// List of coupon line text specifying style of a coupon printed at Till
        /// </summary>
        /// <remarks>CouponLineTextInfo returns list of coupon lines and their formats to be printed at Till</remarks>
        /// <example>The list contains: LineNumber, LineText, LineUsed, Underline, Italic, WhiteOnBlack, Center, Barcode, CharacterWeight, CharacterWidth</example>
        [DataMember]
        public List<CouponLineTextInfo> ListCouponLineInfo { get; set; }
    }
}