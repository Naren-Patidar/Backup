using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;
using System.Diagnostics;


namespace InstoreClubcardReward.Business
{
    [Serializable]
    public class BookingPrintVoucher : BaseClassPrintVoucher
    {
        public int TransactionID { get; set; }
        // parameters supplied by the calling URL
        public int KioskID { get; set; }
        public string Clubcard { get; set; }
        public string PostCode { get; set; }
        public string AddressLine1 { get; set; }
        public DateTime PrintDate { get; set; }
        public Int32 totalActiveVouchers { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public Int32 Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime TranStartTime { get; set; }
        public string StatusLoginAttempts { get; set; }
        public int StoreID { get; set; }
        public int KioskNo { get; set; }

        public Int32 CouponStatusID { get; set; }

        public DateTime? DOB { get; set; }
        public string SSN { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        
        public string CouponDescription { get; set; }        
        public string SmartBarcode { get; set; }        
        public string SmartAlphaCode { get; set; }        
        public DateTime? CouponExpiryDate { get; set; }
        public int? ActiveCoupons { get; set; }
        public Int32 totalActiveCoupons { get; set; }


        public UnusedVoucherCollection UnusedVouchers { get; set; }
        public UnusedCouponCollection UnusedCoupons { get; set; }

        public BookingPrintVoucher()
        {
            UnusedVouchers = new UnusedVoucherCollection();
            UnusedCoupons = new UnusedCouponCollection();
        }

        

        

        
      
        /// <summary>
        /// Saves to booking table. Public for testing.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public int SaveToBookingTable()
        {
            InstoreClubcardReward.Data.InsertPrintTransaction ib = new InstoreClubcardReward.Data.InsertPrintTransaction(ConnectionString);
            
            //int transactionID = 0;
            try
            {
                ib.Clubcard = this.Clubcard;
                ib.KioskId = (SqlInt32)this.KioskID;
                ib.StatusId =(SqlInt32)this.Status;
                ib.TranStartTime = this.TranStartTime;
                ib.CouponStatusID = this.CouponStatusID;
                ib.Execute(this.TransactionID);
                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return (int)ib.TransactionID;
        }

        /// <summary>
        /// Processes the booking.
        /// </summary>
        /// <returns>Boolean to indicate whether the processing was successful</returns>
        public int ProcessBooking()
        {
            try
            {
                this.TransactionID = this.SaveToBookingTable();
                return TransactionID;

            }
            catch (Exception ex)
            {
                this.SaveToTransError("SV EXCEPTION : BookingPrintVoucher - ProcessBooking - " + ex.ToString());
                return 0;
            }
        }


        
        /// <summary>
        /// Saves to Transaction_Error table.
        /// </summary>
        /// <param name="status">error</param>
        /// <returns></returns>
        public void SaveToTransError(string errorDesc)
        {

            if (this.TransactionID != 0)
            {
                try
                {
                    InstoreClubcardReward.Data.InsertTransError ite = new InstoreClubcardReward.Data.InsertTransError(ConnectionString);
                    ite.TransactionID = this.TransactionID;
                    ite.ErrorDesc = errorDesc;
                   // ite.ErrorDateTime = DateTime.Now;
                    ite.Execute();
                }
                catch (Exception ex)
                {
                    // this saves the error trying to be saved
                    EventLog objEventLog = new EventLog("Application");
                    objEventLog.Source = "ICCR";
                    objEventLog.WriteEntry(string.Format("VoucherPrintAtKiosk - SaveToTransError. TransactionID {0}, Error: {1}", TransactionID, ex.ToString()));

                    // If the saving fails to save the error then supress the error
                }
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public void UpdateTranDetailsActiveVoucher()
        {

            if (this.TransactionID != 0)
            {
                try
                {
                    InstoreClubcardReward.Data.UpdateTranDetailsActiveVoucher uta = new InstoreClubcardReward.Data.UpdateTranDetailsActiveVoucher(ConnectionString);
                    uta.TransactionID = this.TransactionID;
                    uta.TotalActiveVouchers = (SqlInt32)this.totalActiveVouchers;
                    uta.TotalActiveCoupons = (SqlInt32)this.totalActiveCoupons;
                    uta.Execute();
                }
                catch (Exception ex)
                {
                    this.SaveToTransError("BookingPrintVoucher - UpdateTranDetailsActiveVoucher - " + ex.ToString());
                    throw ex;
                }
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public void UpdateTranDetailsStatus()
        {

            if (this.TransactionID != 0)
            {
                try
                {
                    InstoreClubcardReward.Data.UpdateTranDetailsStatus uts = new InstoreClubcardReward.Data.UpdateTranDetailsStatus(ConnectionString);
                    uts.TransactionID = this.TransactionID;
                    uts.Status = (SqlInt32) this.Status;
                    uts.CouponStatus = (SqlInt32)this.CouponStatusID;
                    uts.Execute();
                }
                catch (Exception ex)
                {
                    this.SaveToTransError("BookingPrintVoucher - UpdateTranDetailsStatus - " + ex.ToString());
                    throw ex;
                }
            }

        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public void UpdateTranDetailsPrintDate()
        {

            if (this.TransactionID != 0)
            {
                try
                {
                    InstoreClubcardReward.Data.UpdateTranDetailsPrintDate utpd = new InstoreClubcardReward.Data.UpdateTranDetailsPrintDate(ConnectionString);
                    utpd.TransactionID = this.TransactionID;
                    utpd.Status = (SqlInt32)this.Status;
                    utpd.CouponStatus = (SqlInt32)this.CouponStatusID;
                    utpd.PrintDate = this.PrintDate;
                    utpd.Execute();
                }
                catch (Exception ex)
                {
                    this.SaveToTransError("BookingPrintVoucher - UpdateTranDetailsPrintDate - " + ex.ToString());
                    throw ex;
                }
            }

        }
        

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public void UpdateTranDetailsVerifiedStartTime()
        {

            if (this.TransactionID != 0)
            {
                try
                {
                    InstoreClubcardReward.Data.UpdateTranDetailsVerifiedStartTime utvt = new InstoreClubcardReward.Data.UpdateTranDetailsVerifiedStartTime(ConnectionString);
                    utvt.TransactionID = this.TransactionID;
                    utvt.Status = (SqlInt32)this.Status;
                    utvt.StartTime = this.StartTime;
                    utvt.Execute();
                }
                catch (Exception ex)
                {
                    this.SaveToTransError("SV EXCEPTION : BookingPrintVoucher - UpdateTranDetailsVerifiedStartTime - " + ex.ToString());
                    throw ex;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public void UpdateTranDetailsVerifiedTime()
        {

            if (this.TransactionID != 0)
            {
                try
                {
                    InstoreClubcardReward.Data.UpdateTranDetailsVerifiedTime utvt = new InstoreClubcardReward.Data.UpdateTranDetailsVerifiedTime(ConnectionString);
                    utvt.TransactionID = this.TransactionID;
                    utvt.Status = (SqlInt32)this.Status;
                    utvt.EndTime = this.EndTime;
                    utvt.Execute();
                }
                catch (Exception ex)
                {
                    this.SaveToTransError("SV EXCEPTION : BookingPrintVoucher - UpdateTranDetailsVerifiedTime - " + ex.ToString());
                    throw ex;
                }
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public void InsertLoginAttempts()
        {

            if (this.TransactionID != 0)
            {
                try
                {
                    InstoreClubcardReward.Data.InsertLoginAttempts loginAttempts = new InstoreClubcardReward.Data.InsertLoginAttempts(ConnectionString);
                    loginAttempts.TransactionID = this.TransactionID;
                    loginAttempts.ClubcardNo = this.Clubcard;
                    loginAttempts.Status = this.StatusLoginAttempts;
                    loginAttempts.PostCode = this.PostCode;
                    loginAttempts.AddressLine1 = this.AddressLine1;

                    loginAttempts.FirstName = this.FirstName;
                    loginAttempts.LastName = this.Surname;
                    loginAttempts.DOB = this.DOB.HasValue ? this.DOB.Value : SqlDateTime.Null;
                    loginAttempts.SSN = this.SSN;
                    loginAttempts.Email = this.Email;
                    loginAttempts.MobileNo = this.MobileNo;

                    loginAttempts.Execute();
                }
                catch (Exception ex)
                {
                    this.SaveToTransError("BookingPrintVoucher - InsertLoginAttempts - " + ex.ToString());
                    throw ex;
                }
            }

        }
        

    }
}
