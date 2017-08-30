using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BigExchange
{
    /// <summary>
    /// Created Date : 08/06/2011
    /// Created By: Seema Kudari
    /// Class Name: ProductLineCollection
    /// </summary>
   
    [CollectionDataContract]
    public class ProductLineCollection : BindingList<ProductLine>
    {

    }
}
