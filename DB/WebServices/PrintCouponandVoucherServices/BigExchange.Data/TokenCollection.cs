using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BigExchange
{
    /// <summary>
    /// Created Date : 08/06/2011
    /// Created By: Seema Kudari
    /// Class Name: TokenCollection
    /// </summary>

    [CollectionDataContract]
    public class TokenCollection : BindingList<Token>
	{
        public TokenCollection()
        {
        }
	}
}
