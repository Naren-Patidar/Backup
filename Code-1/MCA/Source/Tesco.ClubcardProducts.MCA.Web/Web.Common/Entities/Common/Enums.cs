using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common
{

    public enum VoucherSchemeName
    {
        Voucher = 0,
        XmasSaver = BusinessConstants.XMASSAVER,
        Avios = BusinessConstants.AIRMILES_STD,
        AviosPremium = BusinessConstants.AIRMILES_PREMIUM,
        Virgin = BusinessConstants.VIRGIN,
        BAAvios = BusinessConstants.BAMILES_STD,
        BAAviosPremium = BusinessConstants.BAMILES_PREMIUM
    }

}
