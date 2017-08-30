namespace InstoreClubcardReward.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public enum VoucherTypes
    {
        // only valid voucher types for using 
        // for double up should be here UK
        Unknown = 0,    // this has to be checked for as unknown is on the list
        Tesco = 11,
        TPF = 22,
        XmasClub = 33,
        // staff offer
        StaffOffer = 05,

        // ROI
        Clubcard_ROI_Lo = 50,
        Clubcard_ROI_Hi = 51,
        TPF_ROI_Lo = 52,
        TPF_ROI_Hi = 53,
        XmasSaver_ROI_Lo = 54,
        XmasSaver_ROI_Hi = 55

    }
}
