namespace InstoreClubcardReward.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public class Token : BaseClass
    {
        /// For year long exchange
        private const int YEARLONGEX_MONTHS = 6;
        /// part of product parameter in freetime
        private const int MONTHS = 3;
        /// The extra buffer in days 2009/ Aug 2010 set to 1 and hardcoded
        /// hidden from customer
        private const int DT_EXTRADAY = 0;

        /// this is the customer value (token value)
        public int TokenValue { get; set; }         // value is in pence

        public string EAN { get; set; }
        public string Alpha { get; set; }           // future, record anyway
        public string ProductCode { get; set; }

        public DateTime UsedByDate { get; set; }    // used by date is from the product collection (from Product table)
                                                    // this is used if less than the rolling + 3 months
        public int VendorCode { get; set; }         // from products and used for creating / cancelling tokens 
        /// supply date is the reference datetime - written to database and used in calculations
        private DateTime _SupplyDate;
        public DateTime SupplyDate
        {
            get
            {
                // on first get set the date (avoid any issues if calls on dates over midnight)
                if (_SupplyDate == new DateTime())
                {
                    _SupplyDate = DateTime.Now;
                }
                return _SupplyDate;

            }
            // used when reading value for reprint
            set
            {
                _SupplyDate = value;
            }

        }

        public int ProductLineId { get; set; }      // calculated when creating tokens from product requirements
        public int TokenId { get; set; }            // return from creation of table token

        
        public int ResponseCode { get; set; }   // return from the creation               
        /// return from creation record on suppliertokencodeid
        /// or return from dotcom code that is used                            
        public int SupplierTokenCodeId { get; set; }   
 
        /// used when reading back from database for reprint
        public DateTime CustomerDate { get; set; }
        public DateTime EndDate { get; set; } 

        ///constructor
        public Token()
        {
        }

        public Token(string ean)
        {
            this.EAN = ean;
        }

        /// constructor starting with the token (table TOKEN data)
        public Token(int productlineid)
        {
            this.ProductLineId = productlineid;
        }

        /// constructor used for reprint
        public Token(string alpha,
                                    string ean,
                                    string productcode,
                                    DateTime supplydate,
                                    DateTime customerdate,
                                    DateTime enddate,
                                    int productlineid,
                                    int tokenid)
        {

            this.Alpha = alpha;
            this.EAN = ean;

            this.ProductCode = productcode;
            this.SupplyDate = supplydate;
            this.CustomerDate = customerdate;
            this.EndDate = enddate;

            this.ProductLineId = productlineid;
            this.TokenId = tokenid;
        }


        /// value was fixed. Now it is assigned
        /// note value is in in £'s rather than pence
        public double GetTokenCustomerValue()
        {
            // from ean get the token value - not from a call but from form of the ean
            // this was hard coded now read from TokenValue (set when creating tokens)
            return this.TokenValue / 100.0;
        }


        public int GetTokenType()
        {
            // from ean get the type - from a call but from form of the ean
            // TBD################ Check indexing
            return int.Parse(EAN.Substring(2, 2));
        }

        /// get customer date 3 months on - displayed on the token
        public string GetCustomerDate()       // 3 months 
        {
            return GetBaseExpiryDate().ToString("dd-MM-yyyy");
        }

        /// get expiry date - sent as parameter for double Up token generation
        public string GetExpiryDate()         // 3 months on + 1 day
        {
            return GetExpiryDateDT().ToString("yyyy-MM-dd");
        }
        /// date representation - stored in SupplierTokenCode
        public DateTime GetExpiryDateDT()         // 3 months on + 1 day
        {
            return GetBaseExpiryDate().AddDays(DT_EXTRADAY);
        }

        /// base expiry date works from the supply date
        /// and adds 3 months, but may be overriden by the UsedByDate
        /// this date is the date relevant for the customer
        public DateTime GetBaseExpiryDate()
        {
            if (UsedByDate == new DateTime())
            {
                // no override date so simple + 3 months
                return SupplyDate.AddMonths(MONTHS);
            }
            else {
                //Year long run Exchange
                if (UsedByDate == DateTime.Today.AddMonths(YEARLONGEX_MONTHS))
                {
                    return DateTime.Today.AddDays(1).AddMonths(YEARLONGEX_MONTHS);
                }
                // if overrride date is less then use this date
                else if (UsedByDate < SupplyDate.AddMonths(MONTHS) )
                {
                    return UsedByDate;
                }
                else{
                    return SupplyDate.AddMonths(MONTHS);
                }
            }


        }



        /// Token Table Save
        public void SaveToToken( int bookingid)
        {

            InstoreClubcardReward.Data.InsertToken it = new InstoreClubcardReward.Data.InsertToken(ConnectionString);
            it.BookingId = bookingid;
            it.ProductLineId = (System.Data.SqlTypes.SqlInt32)ProductLineId;

            it.Execute();

            // record the tokenid
            this.TokenId = (int) it.TokenId;

        }

        /// SupplerTokenCodeSave
        public void SaveToSupplierTokenCode()
        {

            InstoreClubcardReward.Data.InsertSupplierTokenCode istc = new InstoreClubcardReward.Data.InsertSupplierTokenCode(ConnectionString);
            istc.SupplierCode = this.ProductCode;
            istc.SupplierTokenId = this.Alpha;
            istc.SupplierTokenCode = this.EAN;
            istc.SupplyDate =  this.SupplyDate;
            istc.CustomerDate = DateTime.Parse(this.GetCustomerDate());
            istc.TokenId = this.TokenId;
            istc.EndDate = this.GetExpiryDateDT();
            istc.Status = null; // not set on creation - use for cancellation

            istc.Execute();

            this.SupplierTokenCodeId = (int) istc.Id;

        }
        
    }
}
