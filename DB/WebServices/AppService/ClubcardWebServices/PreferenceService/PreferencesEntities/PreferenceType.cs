using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
        [DataContract]
        public enum PreferenceType
        {
            /// <remarks/>
            [EnumMember]
            NULL = 0,

            /// <remarks/>
            [EnumMember]
            ALLERGY = 1,

            /// <remarks/>
            [EnumMember]
            DIETARY = 2,

            /// <remarks/>
            [EnumMember]
            CONTACT_METHOD = 3,

            /// <remarks/>
            [EnumMember]
            PROMOTIONS = 4,

            /// <remarks/>
            [EnumMember]
            REWARD = 5,

            /// <remarks/>
            [EnumMember]
            PREFERRED_MAILING_ADDRESS = 6,

            /// <remarks/>
            [EnumMember]
            MEDICAL = 7,

            /// <remarks/>
            [EnumMember]
            COMMUNICATION_LANGUAGE = 8,

            /// <remarks/>
            [EnumMember]
            DATA_PROTECTION = 9,
        }
}