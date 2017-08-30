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
    /// Class Name: PrintReason
    /// </summary>
    [DataContract]
    public class PrintReason
    {

        [DataMember]
        public int? PrintReasonId { get; set; }
       
        [DataMember]
        public int? DisplayOrder { get; set; }
      
        [DataMember]
        public string PrintReasonText { get; set; }

        [DataMember]
        public bool? Enabled { get; set; }
    
        public PrintReason()
        {
        }

    }
}
