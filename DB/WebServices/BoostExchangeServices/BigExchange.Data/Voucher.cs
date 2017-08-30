using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace BigExchange
{
    [DataContract]
    public class Voucher
    {
        [DataMember]
        public string Ean { get; set; }
        [DataMember]
        public string Alpha { get; set; }
        [DataMember]
        public string Clubcard { get; set; }   
        [DataMember]
        public string Country { get; set; }    
        [DataMember]
        public VoucherStatus Status { get; set; }
        [DataMember]
        public VoucherTypes Type { get; set; }
        [DataMember]
        public int StoreNo { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string VirtualStore { get; set; }
        [DataMember]
        public DateTime UseDateTime { get; set; }
        [DataMember]
        public DateTime ExpiryDate { get; set; }
        [DataMember]
        public string ResponseCode { get; set; }
        [DataMember]
        public string ResponseClubcard { get; set; }
        [DataMember]
        public decimal ResponseValue { get; set; } 
        [DataMember]
        public bool IsUsed { get; set; }
        [DataMember]
        public int VoucherId { get; set; }
        [DataMember]
        public int Value { get; private set; } 

    }
}
