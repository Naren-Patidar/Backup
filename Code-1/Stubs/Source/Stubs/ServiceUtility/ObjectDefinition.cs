using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceUtility
{
    public class CMetaDataCollection
    {
        public List<CMetadata> Customers { get; set; }
    }

    public class CMetadata
    {
        public string Culture { get; set; }
        public string customerID { get; set; }
        public string clubcardNumber { get; set; }
    }
}