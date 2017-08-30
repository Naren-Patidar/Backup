using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClubcardService
{
    [DataContract]
    public enum AccountIDType : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        CustomerID = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ClubcardID = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        HouseholdID = 2,
    }
}