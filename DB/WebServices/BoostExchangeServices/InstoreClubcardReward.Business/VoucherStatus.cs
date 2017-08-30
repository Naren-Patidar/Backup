namespace InstoreClubcardReward.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public enum VoucherStatus : int
    {
        Active = 0,
        Reserved = 1,
        Redeemed = 2,
        RedeemedAlready = -2, //' online booking vouchers may have already been redeemed (FREETIME CREATED STATUS)
        NotFound = -1,
        Expired = 3,
        Suspended = 4,       //' void - dotcom spec
        Cancelled = 5,
        Reissued = 6,
        RolledOver = 7,
        ProcessedOffline = 100,  //'used when Freetime are unable to contact NGC to process vouchers
        NotChecked = -3,        //' not checked

    } 
}
