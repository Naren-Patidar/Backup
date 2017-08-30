using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CustomerService
{
    [DataContract(Name = "ConsumerSource")]
    public enum ConsumerSource : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Mobile = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Dotcom = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Kiosk = 2,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        MCA = 3,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Other = 4,
    }
}