using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CustomerService
{
    [DataContract(Name = "RedeemType")]
    public enum RedeemType : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Redeem = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        UnRedeem = 1,
    }
}