using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// ClubcardCoupon CouponInformation Entity
    /// </summary>
    [DataContract(Name = "CouponInformation")]
    public class CouponInformation : CouponClass
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
        public string SmartAlphaNumeric { get; set; }

        /// <summary>
        /// Represents the Status of a Coupon 
        /// </summary>
        /// <remarks>Returns integer as redemption coupon</remarks>
        /// <example>Returns 15, if a coupon is successfully redeemed</example>
        [DataMember]
        public int? CouponStatusId { get; set; }

        /// <summary>
        /// Represents the Issuance date of a Coupon
        /// </summary>
        /// <remarks>Returns date time of when the coupon is issued</remarks>
        /// <example>dd/MM/YYYY</example>
        [DataMember]
        public DateTime? IssuanceDate { get; set; }

        /// <summary>
        /// Represents the Store where the Coupon is Issued
        /// </summary>
        /// <remarks>Returns Storenumber as integer where the coupon is issued </remarks>
        /// <example>Any Positive integer</example>
        [DataMember]
        public Int16? IssuanceStore { get; set; }

        /// <summary>
        /// Represents the Issuance Channel of a Coupon
        /// </summary>
        /// <remarks>Returns integer for a specific coupon issuance channel</remarks>
        /// <example>LCM, Statement, Till</example>
        [DataMember]
        public Int16? CouponIssuanceChannel { get; set; }

        /// <summary>
        /// Represents how many times a Coupon is Redeemed
        /// </summary>
        /// <remarks>Returns integer of how many times a coupon is actually redeemed out of its max redemption limit</remarks>
        /// <example>If max redemption limit of a coupon is 5, If customer redeems the coupon 3 times then redemption utilized is 3</example>
        [DataMember]
        public Int16 RedemptionUtilized { get; set; }

        /// <summary>
        /// Represents the Coupon instanceId of a Coupon
        /// </summary>
        /// <remarks>Any positive integer, which will be unique for a customer</remarks>
        /// <example>50013</example>
        public Int64 CouponInstanceId { get; set; }

        /// <summary>
        /// Represents the HouseHold Id of a customer
        /// </summary>
        /// <remarks>Any positive integer, which will be unique for a customer</remarks>
        /// <example>987654321</example>
        [DataMember]
        public string HouseholdId { get; set; }

        /// <summary>
        /// ListRedemptionInfo will return List of redeemed coupons
        /// </summary>
        /// <remarks>Returns list of redeemed coupons information</remarks>
        /// <example>The list contains: ClubcardNumber, ChannelCode, StoreNumber, TillId, TillType, Redemptiondatetime, CashierNumber, IsOffline, RedemptionType, CouponInstanceID</example>
        [DataMember]
        public List<RedemptionInfo> ListRedemptionInfo { get; set; }
    }

    /// <summary>
    /// ClubcardCoupon RedemptionInfo Entity
    /// </summary>
    [DataContract]
    public class RedemptionInfo
    {
        /// <summary>
        /// Represents the ClubcardNumber number of a Customer
        /// </summary>
        /// <example>63465780954678934</example>
        [DataMember]
        public string ClubcardNumber { get; set; }

        /// <summary>
        /// Represents the Redemption Channel
        /// </summary>
        /// <remarks>Returns integer as Channel code where the coupon can be redeemed</remarks>
        /// <example>Returns 1 for dotcom redemption, 2 for Storeline, 3 for Others</example>
        [DataMember]
        public Int16? ChannelCode { get; set; }

        /// <summary>
        /// Represents StoreNumber of Coupon where the Coupon is Redeemed
        /// </summary>
        /// <remarks>Returns integer as StoreNumber, where the coupon is redeemed</remarks>
        /// <example>Any Positive integer</example>
        [DataMember]
        public Int16? StoreNumber { get; set; }

        /// <summary>
        /// Represents TillId of the Redeemed Coupon
        /// </summary>
        /// <remarks>Returns integer as TillId, where the coupon is redeemed</remarks>
        /// <example>Any Positive integer</example>
        [DataMember]
        public Int16? TillId { get; set; }

        /// <summary>
        /// Represents TillType where the coupon is redeemed
        /// </summary>
        /// <remarks>Returns integer for Till Type</remarks>
        /// <example>Any Positive integer</example>
        [DataMember]
        public Int16? TillType { get; set; }

        /// <summary>
        /// Represents DateTime of coupon redemption
        /// </summary>
        /// <remarks>Specifies the datetime of when a coupon is redeemed</remarks>
        /// <example>dd/MM/YYYY</example>
        [DataMember]
        public DateTime RedemptionDateTime { get; set; }

        /// <summary>
        /// Represents CashierNumber who collected cash from the Customer
        /// </summary>
        /// <remarks>Returns string for CashierNumber</remarks>
        /// <example>101</example>
        [DataMember]
        public string CashierNumber { get; set; }

        /// <summary>
        /// Returns true if the transaction is Offline
        /// </summary>
        /// <remarks>Returns true if the transcation is Offline, else false</remarks>
        /// <example>True/False</example>
        [DataMember]
        public bool IsOffline { get; set; }

        /// <summary>
        /// Represents the type of Redemption
        /// </summary>
        /// <remarks>Returns 0 if a coupon is redeemed and 1 if a coupon is unredeemed</remarks>
        /// <example>0/1</example>
        [DataMember]
        public RedeemType RedemptionType { get; set; }

        /// <summary>
        /// Represents CouponInstanceId of a Coupon
        /// </summary>
        /// <remarks>Any positive integer, CouponinstanceId will be unique for a customer</remarks>
        /// <example>45309</example>
        public Int64 CouponInstanceId { get; set; }
    }

    /// <summary>
    /// ClubcardCoupon RedeemType enumeration
    /// </summary>
    [DataContract]
    public enum RedeemType
    {
        /// <summary>
        /// Enumeration for redeem type
        /// </summary>
        /// <remarks>If a coupon is redeemed, returns 0</remarks>
        /// <example>0</example>
        [EnumMember]
        Redeem = 0,
        
        /// <summary>
        /// Enumeration for redeem type
        /// </summary> 
        /// <remarks>If a coupon is unredeemed, returns 1</remarks>
        /// <example>1</example>
        [EnumMember]
        UnRedeem = 1
    }
}