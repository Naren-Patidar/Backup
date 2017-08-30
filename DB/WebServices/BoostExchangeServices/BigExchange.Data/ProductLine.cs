using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;



namespace BigExchange
{
    /// <summary>
    /// Created Date : 08/06/2011
    /// Created By: Seema Kudari
    /// Class Name: ProductLine
    /// </summary>
  
    [DataContract]
    public class ProductLine
    {
        [DataMember]
        public Product Product;
        [DataMember]
        public int ProductNumber { get; set; }
        [DataMember]
        public int ProductLineId { get; set; }
        [DataMember]
        public TokenCollection Tokens { get; set; }

        /// <summary>
        /// Initializes a new instance of the ProductLine class.
        /// </summary>
        public ProductLine()
        {
            Tokens = new TokenCollection();
        }

          
    }
}
