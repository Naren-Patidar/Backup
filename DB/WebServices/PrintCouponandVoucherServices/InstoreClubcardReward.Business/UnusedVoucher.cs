namespace InstoreClubcardReward.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Configuration;

    [Serializable]
    public class UnusedVoucher : BaseClassPrintVoucher
    {
        //public string Clubcard { get; set; } 
        public string HouseholdId { get; set; }
        public string PeriodName { get; set; }
        public Decimal VoucherValue { get; set; }
        public string VoucherNumber { get; set; }
        public string OnlineCode { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int VoucherType { get; set; }
        public string Ean { get; set; }

        ///constructor
        public UnusedVoucher()
        {

        }

        ///constructor

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
        /// save to the database the individual voucher voucherid returned
        /// if VoucherId is non zero ie has been saved so update is used
        public void Save(int transactionID)
        {
            // insert
            InstoreClubcardReward.Data.InsertVoucherDetails ivd = new InstoreClubcardReward.Data.InsertVoucherDetails(ConnectionString);
            ivd.TransactionID = transactionID;
            ivd.EAN = this.Ean;
            ivd.VoucherValue = this.VoucherValue;

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