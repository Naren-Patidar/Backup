using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace BigExchange
{
    /// <summary>
    /// Created Date : 08/06/2011
    /// Created By: Dimple Kandoliya
    /// Class Name: CategoryIncludes
    /// </summary>
    [DataContract]
    public class CategoryIncludes
    {
        [DataMember]
        public int CategoryId { get; set; }
        [DataMember]
        public int LineId { get; set; }

        [DataMember]
        public string Description1 { get; set; }
        [DataMember]
        public string Description2 { get; set; }
        [DataMember]
        public string Description3 { get; set; }

        /// <summary>
        /// Initializes a new instance of the CategoryIncludes class.
        /// </summary>
        public CategoryIncludes()
        {

        }

    }
}
