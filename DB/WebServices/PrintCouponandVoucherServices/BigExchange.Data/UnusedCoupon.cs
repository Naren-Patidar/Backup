using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace BigExchange
{
    [DataContract]
    public class UnusedCoupon
    {
        [DataMember]
        public string CouponDescription { get; set; }
        [DataMember]
        public string SmartBarcode { get; set; }
        [DataMember]
        public string SmartAlphaCode { get; set; }
        [DataMember]
        public DateTime? CouponExpiryDate { get; set; }
        [DataMember]
        public int? ActiveCoupons { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public UnusedCoupon()
        {
        }

        public UnusedCoupon(string coupondescription, string smartbarcode, string smartalphaCode, DateTime couponexpiryDate, int activecoupons)
        {
            this.CouponDescription = coupondescription;
            this.SmartBarcode = smartbarcode;
            this.SmartAlphaCode = smartalphaCode;
            this.CouponExpiryDate = couponexpiryDate;
            this.ActiveCoupons = activecoupons;
        }


    }

   

}
