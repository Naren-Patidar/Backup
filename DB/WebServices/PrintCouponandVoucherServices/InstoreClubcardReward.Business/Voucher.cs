namespace InstoreClubcardReward.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Configuration;

    [Serializable]
    public class Voucher : BaseClass
    {
        public string Ean { get; set; }
        public string Alpha { get; set; }       // possible for input as alphanumeric when not scanned
        public string Clubcard { get; set; }    // could be from voucher or from customer clubcard
        
        /// possible use by ROI so create country
        public string Country { get; set; }    // could be from voucher or from customer clubcard


        /// info from voucher message
        public VoucherStatus Status { get; set; }

        public VoucherTypes Type { get; set; }


        /// voucher usage - seperate class in VB
        public int StoreNo { get; set; }
        public string Channel { get; set; }
        public string VirtualStore { get; set; }
        public DateTime UseDateTime { get; set; }

        public DateTime ExpiryDate { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseClubcard { get; set; }    // response clubcard
        /// value might need converting look at message to decide type
        public decimal ResponseValue { get; set; }       // response value

        public bool IsUsed { get; set; }    // conditions to use ie active and right type..... avoid using double up to pay for double up

        public int VoucherId { get; set; }    // return from save - can be used for cancelling

        /// Freetime this is a lookup against the database 
        /// spSelVoucherValue dbo.tblVoucherValue
        public int Value
        {
            get
            {
                if (CheckValidLength())
                {

                    // might need to check for alpha if not checking before totaling 

                    // return int.Parse(Ean.Substring(1, 2));
                    if (ResponseValue > 0)
                    {
                        return (int) (ResponseValue * 100);
                    }
                    else
                    {
                        int intPriceCode = int.Parse(Ean.ToString().Substring(7, 3));
                        int intVoucherValue = (intPriceCode - 1) * 50;
                        return intVoucherValue;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
        
        ///constructor
        public Voucher()
        {
            IsUsed = true;
        }

        ///constructor
        public Voucher(string ean) : this()
        {
            this.Ean = ean;
            // default to not checked
            this.Status = VoucherStatus.NotChecked;
        }

        public Voucher(string ean, string clubcard) : this(ean)
        {
            //this.EAN = ean;
            this.Clubcard = clubcard;
            //this.Status = VoucherStatus.NotChecked;

        }

        /// checks... size, general construction - without checking with SV
        /// Length 
        public Boolean CheckValidLength()
        {
            if (!string.IsNullOrEmpty(this.Ean))
            {
                if (this.Ean.Length == 22 )
                {
                    return true;
                }
            }
            else if (!string.IsNullOrEmpty(this.Alpha))
            {
                if ( this.Alpha.Length == 12)
                {
                    return true;
                }
            }
            return false;
        }


        /// use before adding (checks EAN... based on codes)
        /// also sets type
        public Boolean CheckValidType()
        {
            // Only allow smart vouchers to be used 
            // (do not allow double up to be used)
            if (CheckValidLength())
            {
                try 
	            {
                    // use set type do check for valid type
                    Type = (InstoreClubcardReward.Business.VoucherTypes) int.Parse(Ean.Substring(2, 2));

                    if (ConfigurationManager.AppSettings["Country"] == "UK")
                    {
                        if (Type == VoucherTypes.Tesco ||
                        Type == VoucherTypes.TPF ||
                        Type == VoucherTypes.StaffOffer)
                        {
                            return true;
                        }
                        else
                        { 
                            return false;
                        }
                    }
                    else if (ConfigurationManager.AppSettings["Country"] == "ROI")
                    {
                        if (Type == VoucherTypes.TPF_ROI_Lo ||
                        Type == VoucherTypes.TPF_ROI_Hi ||
                        Type == VoucherTypes.Clubcard_ROI_Lo ||
                        Type == VoucherTypes.Clubcard_ROI_Hi ||
                        Type == VoucherTypes.StaffOffer)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
	            }
	            catch (Exception)
	            {
                    return false;
	            }
            }
            else
            {
                return false;
            }
        }

        /// get the type from the EAN (allows specific feedback for xmas club)
        public InstoreClubcardReward.Business.VoucherTypes getTypeFromEAN()
        {
            // return the type (defaults to unknown
            if (CheckValidLength())
            {
                try
                {
                    return (InstoreClubcardReward.Business.VoucherTypes)int.Parse(Ean.Substring(2, 2));
                }
                catch (Exception)
                {
                    return VoucherTypes.Unknown;
                }
            }
            else
            {
                return VoucherTypes.Unknown;
            }
        }


        /// return 0 active, -1 no message, rest from SV message
        public int CheckValidSV()
        {
            // call to SV to look for an active voucher
            // could use ean or alpha
            // possible error messages, miscommunication etc
            if (this.Ean.Length == 3)   //#########################
            {
                //SmartCallCollection scc = (SmartCallCollection)System.Web.HttpContext.Current.Session["SmartCall"];
                //scc.Add(new SmartCall("Voucher Check: " + this.EAN));

                // call will need branch posid etc
                return 0;       // Active
            }
            else
            {
                return -1;
            }
        }


        /// save to the database the individual voucher voucherid returned
        /// if VoucherId is non zero ie has been saved so update is used
        public void Save( int bookingid)
        {
            // only save if using
            if (IsUsed)
            {
                if (this.VoucherId == 0)
                {
                    // insert
                    InstoreClubcardReward.Data.InsertPaymentVoucher ipv = new InstoreClubcardReward.Data.InsertPaymentVoucher(ConnectionString);
                    ipv.BookingId  = bookingid;
                    ipv.EAN = this.Ean;
                    ipv.Alpha = this.Alpha;
                    ipv.VoucherType = (int)this.Type;
                    ipv.Amount = this.Value;
                    int i = (int)this.Status;   // TODO two stage conversion ######### CHECK
                    ipv.Status = (System.Data.SqlTypes.SqlInt32) i;
                    try
                    {
                        ipv.Execute();
                    }
                    catch (Exception ex)
                    {
                        // error inserting
                        throw new BookingException(ErrorTypes.InsertVoucher, string.Format("Voucher.Save BookingId {0}, EAN {1}",ipv.BookingId, ipv.EAN, ex));

                    }

                    // record the created voucherid
                    this.VoucherId = (int) ipv.VoucherId;
                }
                else
                {
                    // update
                    InstoreClubcardReward.Data.UpdatePaymentVoucher upv = new InstoreClubcardReward.Data.UpdatePaymentVoucher(ConnectionString);
                    upv.VoucherId = this.VoucherId;
                    int i = (int)this.Status;   // TODO two stage conversion ######### CHECK
                    upv.Status = (System.Data.SqlTypes.SqlInt32)i;

                    try
                    {
                        upv.Execute();
                    }
                    catch (Exception ex)
                    {
                        // error updating
                        throw new BookingException(ErrorTypes.UpdateVoucher, string.Format("Voucher.Save BookingId {0}, EAN {1}", bookingid, this.Ean, ex));

                    }


                }
            }
        }



    }
}
