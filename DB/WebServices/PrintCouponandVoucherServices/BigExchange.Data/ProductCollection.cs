using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BigExchange
{
    [CollectionDataContract]
    public class ProductCollection : BindingList<Product>
    {

    }
}
