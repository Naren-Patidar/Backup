using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;

namespace BigExchange
{
    [CollectionDataContract]
    public class VoucherCollection : BindingList<Voucher>
    {
        public VoucherCollection()
        {
        }
    }
}
