using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BigExchange
{
    [DataContract]
    public class Product
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string StrippedDescription { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public string VendorCode { get; set; }
        [DataMember]
        public int CustomerPrice { get; set; }
        [DataMember]
        public int TokenValue { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string ValidUntil { get; set; }
        [DataMember]
        public DateTime UsedByDate { get; set; }   
        [DataMember]
        public string ImageFilename { get; set; }
        [DataMember]
        public Category Category { get; set; }
        [DataMember]
        public string ShortDescription { get; set; }
        [DataMember]
        public string LongDescription { get; set; }
        [DataMember]
        public ProductType ProductType { get; set; }
        [DataMember]
        public TokenType TokenType { get; set; }
        [DataMember]
        public string TokenTitle { get; set; }
        [DataMember]
        public string TokenDescription { get; set; }
        [DataMember]
        public string TokenTermsAndConditions { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        public Product()
        {
        }

    }
}
