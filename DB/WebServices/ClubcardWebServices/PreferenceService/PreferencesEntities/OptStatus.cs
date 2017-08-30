using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    [Serializable]
    [DataContract]
    public enum OptStatus
    {
        /// <remarks/>
        [EnumMember]
        NOT_SELECTED = 0,

        /// <remarks/>
        [EnumMember]
        OPTED_IN = 1,

        /// <remarks/>
        [EnumMember]
        OPTED_OUT = 2
    }
}
