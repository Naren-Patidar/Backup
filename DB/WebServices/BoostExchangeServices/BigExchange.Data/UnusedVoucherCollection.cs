using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;


namespace BigExchange
{
    /// <summary>
    /// Created Date : 29/12/2011
    /// Created By: Dimple Kandoliya
    /// Class Name: UnusedVoucherCollection
    /// </summary>
    [CollectionDataContract]
    public class UnusedVoucherCollection : BindingList<UnusedVoucher>
    {
        public UnusedVoucherCollection()
        {
        }
    }
}
