using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    [DataContract]
    public enum PreferenceType : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        NULL = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ALLERGY = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        DIETARY = 2,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        CONTACT_METHOD = 3,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        PROMOTIONS = 4,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        REWARD = 5,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        PREFERRED_MAILING_ADDRESS = 6,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        MEDICAL = 7,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        COMMUNICATION_LANGUAGE = 8,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        DATA_PROTECTION = 9,
    }
}