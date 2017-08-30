using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace BigExchange
{
    /// <summary>
    /// Created Date : 08/06/2011
    /// Created By: Dimple Kandoliya
    /// Class Name: ErrorTypes
    /// </summary>


    [DataContract]
    public enum ErrorTypes
        {
            [EnumMember]
            NoError = 0,
            [EnumMember]
            CancelBooking,
            [EnumMember]
            CancelPayment,
            [EnumMember]
            CancelToken,
            [EnumMember]
            CreateTillCoupons,
            [EnumMember]
            CreateDotComToken,
            [EnumMember]
            CreateToken,
            [EnumMember]
            CreateTokenForClubcard,
            [EnumMember]
            HouseholdBlankResponse,
            [EnumMember]
            HouseholdCardNotFound,
            [EnumMember]
            HouseholdSkeleton,
            [EnumMember]
            HouseholdMissingVouchers,
            [EnumMember]
            InsertBooking,
            [EnumMember]
            InsertChangePayment,
            [EnumMember]
            InsertPrint,
            [EnumMember]
            InsertProductLine,
            [EnumMember]
            InsertVoucher,
            [EnumMember]
            InvalidPaymentResponse,
            [EnumMember]
            InvalidTokenResponse,
            [EnumMember]
            ProcessPayment,
            [EnumMember]
            UpdateBooking,
            [EnumMember]
            UpdateVoucher,
            [EnumMember]
            BookingProcessError

        }
}
