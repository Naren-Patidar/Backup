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
    /// Enum Name: TokenType
    /// </summary>

      [DataContract]
    public enum TokenType
    {
           [EnumMember]
        Token = 1,
           [EnumMember]
        Direct = 2,
           [EnumMember]
        Online = 3

    }
}
