using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    [DataContract]
    public enum OptStatus : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        NOT_SELECTED = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        OPTED_IN = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        OPTED_OUT = 2,
    }

}