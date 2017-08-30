using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BigExchange
{
    /// <summary>
    /// Created Date : 08/06/2011
    /// Created By: Dimple Kandoliya
    /// Class Name: Category
    /// </summary>
    [DataContract]
    public class Category
    {
        [DataMember]
        public int CategoryId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ImageFilename { get; set; }
        [DataMember]
        public Category Parent { get; set; }
        [DataMember]
        public int? TokenValue { get; set; }
        // token value in pence (max value for products in category, or null if no product)

        /// <summary>
        /// Initializes a new instance of the Category class.
        /// </summary>
        public Category()
        {

        }

    }
}
