using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace BigExchange
{
    /// <summary>
    /// Created Date : 08/06/2011
    /// Created By: Seema Kudari
    /// Class Name: ProductTokenMakeupCollection
    /// </summary>
    
    [CollectionDataContract]
    public class ProductTokenMakeupCollection : BindingList<ProductTokenMakeup>
    {
    }
}
