using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BigExchange
{
    /// <summary>
    /// Created Date : 08/06/2011
    /// Created By: Seema Kudari
    /// Enum Name: ProductType
    /// </summary>
    
    [DataContract]
    public enum ProductType
    {
         [EnumMember]
        Instore = 1,
         [EnumMember]
        DotCom = 2,
         [EnumMember]
        Rewards = 3
    }
}
