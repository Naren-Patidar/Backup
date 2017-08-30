using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstoreClubcardReward.Business
{
    [Serializable]
    public enum ErrorTypes
    {
        NoError = 0,
        CancelBooking,
        CancelPayment,
        CancelToken,
        CreateTillCoupons,
        CreateDotComToken,
        CreateToken,
        CreateTokenForClubcard,
        HouseholdBlankResponse,
        HouseholdCardNotFound,
        HouseholdSkeleton,
        HouseholdMissingVouchers,
        InsertBooking,
        InsertChangePayment,
        InsertPrint,
        InsertProductLine,
        InsertVoucher,
        InvalidPaymentResponse,
        InvalidTokenResponse,
        ProcessPayment,
        UpdateBooking,
        UpdateVoucher,
        BookingProcessError,
        ValidateBonusVouchers

    }
}
