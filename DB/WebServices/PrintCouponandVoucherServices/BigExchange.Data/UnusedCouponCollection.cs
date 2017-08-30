using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace BigExchange
{
    [CollectionDataContract]
    public class UnusedCouponCollection:BindingList<UnusedCoupon>
    {
        public UnusedCouponCollection()
        {
        }
    }
}
