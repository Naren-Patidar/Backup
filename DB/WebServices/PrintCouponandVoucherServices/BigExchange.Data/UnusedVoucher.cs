using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace BigExchange
{
    /// <summary>
    /// Created Date : 29/12/2011
    /// Created By: Dimple Kandoliya
    /// Class Name: UnusedVoucher
    /// </summary>
    [DataContract]
    public class UnusedVoucher
    {

        [DataMember]
        public string HouseholdId { get; set; }
        [DataMember]
        public string PeriodName { get; set; }
        [DataMember]
        public Decimal VoucherValue { get; set; }
        [DataMember]
        public string VoucherNumber { get; set; }
        [DataMember]
        public string OnlineCode { get; set; }
        [DataMember]
        public DateTime ExpiryDate { get; set; }
        [DataMember]
        public int VoucherType { get; set; }
        [DataMember]
        public string Ean { get; set; }

        ///constructor
        public UnusedVoucher()
        {
        }
        public UnusedVoucher(string householdId, string periodName, Decimal voucherValue, string voucherNumber, string onlineCode, DateTime expiryDate, int voucherType, string ean)
        {
            this.HouseholdId = householdId;
            this.PeriodName = periodName;
            this.VoucherValue = voucherValue;
            this.VoucherNumber = voucherNumber;
            this.OnlineCode = onlineCode;
            this.ExpiryDate = expiryDate;
            this.VoucherType = voucherType;
            this.Ean = ean;

        }
    }
}
