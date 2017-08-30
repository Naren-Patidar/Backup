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
    /// Class Name: Store
    /// </summary>

    [DataContract]
    public class Store
    {
         [DataMember]
        public int StoreNo { get; set; }
         [DataMember]
        public string StoreName { get; set; }
         [DataMember]
        public bool Enabled { get; set; }

        public Store()
        {
        }
    }
}
