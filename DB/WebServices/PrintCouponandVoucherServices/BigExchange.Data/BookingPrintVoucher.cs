using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace BigExchange
{
    /// <summary>
    /// Created Date : 29/12/2011
    /// Created By: Dimple Kandoliya
    /// Class Name: BookingPrintVoucher
    /// </summary>
    [DataContract]
    public class BookingPrintVoucher
    {
        [DataMember]
        public int TransactionID { get; set; }

        [DataMember]
        public int KioskID { get; set; }
        [DataMember]
        public int StoreID { get; set; }
        [DataMember]
        public int KioskNo { get; set; }

        [DataMember]
        public string Clubcard { get; set; }
        [DataMember]
        public string PostCode { get; set; }
        [DataMember]
        public string AddressLine1 { get; set; }
        [DataMember]
        public DateTime PrintDate { get; set; }
        [DataMember]
        public Int32 totalActiveVouchers { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public Int32 Status { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public DateTime EndTime { get; set; }
        [DataMember]
        public DateTime TranStartTime { get; set; }
        [DataMember]
        public string StatusLoginAttempts { get; set; }
        
        [DataMember]
        public UnusedVoucherCollection UnusedVouchers { get; set; }

        [DataMember]
        public UnusedCouponCollection UnusedCoupons { get; set; }

        

       

        //added for international kiosk
        [DataMember]
        public Int32 CouponStatusID { get; set; }
        [DataMember]
         public DateTime? DOB { get; set; }
        [DataMember]
         public string SSN { get; set; }
        [DataMember]
         public string Email { get; set; }
        [DataMember]
         public string MobileNo { get; set; }
        [DataMember]
        public Int32 totalActiveCoupons { get; set; }
       

        public BookingPrintVoucher()
        {
            UnusedVouchers = new UnusedVoucherCollection();
            UnusedCoupons = new UnusedCouponCollection();
        }     

        

    }
}
