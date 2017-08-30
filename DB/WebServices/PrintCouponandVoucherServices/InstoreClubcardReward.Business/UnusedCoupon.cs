using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstoreClubcardReward.Business
{
    [Serializable]
    public class UnusedCoupon:BaseClassPrintVoucher
    {        
        public string CouponDescription { get; set; }        
        public string SmartBarcode { get; set; }        
        public string SmartAlphaCode { get; set; }        
        public DateTime? CouponExpiryDate { get; set; }        
        public int? ActiveCoupons { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public UnusedCoupon()
        {
        }

        public UnusedCoupon(string coupondescription, string smartbarcode, string smartalphacode, DateTime couponexpirydate, int activecoupons)
        {
            this.CouponDescription = coupondescription;
            this.SmartBarcode = smartbarcode;
            this.SmartAlphaCode = smartalphacode;
            this.CouponExpiryDate = couponexpirydate;
            this.ActiveCoupons = activecoupons;
        }

        //call insertcoupondetails of data to insert the details
        public void Save(int transactionID)
        {
            // insert
            InstoreClubcardReward.Data.InsertCouponDetails ivd = new InstoreClubcardReward.Data.InsertCouponDetails(ConnectionString);
            ivd.TransactionID = transactionID;
            ivd.EAN = this.SmartBarcode;
            ivd.CouponDescription = this.CouponDescription;

            try
            {
                ivd.Execute();
            }
            catch (Exception ex)
            {
                // error inserting
                BookingPrintVoucher bpv = new BookingPrintVoucher();
                bpv.SaveToTransError("UnusedVoucher - Save - " + ex.ToString());
                throw ex;//new BookingException(ErrorTypes.InsertVoucher, string.Format("Voucher.Save BookingId {0}, EAN {1}", ipv.BookingId, ipv.EAN, ex));

            }

            //// record the created voucherid
            //this.VoucherId = (int)ipv.VoucherId;

            
        }

    }
}
