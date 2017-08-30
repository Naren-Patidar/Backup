using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InstoreClubcardReward.NGC.Freetime.AuthorisationGatewayAdapter;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;
using InstoreClubcardReward.Data;
using System.Collections.ObjectModel;

namespace InstoreClubcardReward.Business
{

    /// <summary>
    /// 
    /// </summary>
    /// 
    [Serializable]
    public class Booking : BaseClass
    {
        private const string currencyFormat = "0.00";

        public int BookingId { get; set; }
        // parameters supplied by the calling URL
        public int StoreId { get; set; }
        public int UserId { get; set; }
        public int TillId { get; set; }

        // result of the menu choice. Instore or Direct
        // used to determine the products available
        public int BookingType { get; set; }


        public VoucherCollection Vouchers { get; set; }
        public ProductCollection Products { get; set; }
        public ProductLineCollection ProductLines { get; set; }



        // from payment get a clubcard number to use
        // requires redeem messages to be done.... select first? select most frequent?
        public string Clubcard { get; set; } ///// TBD more needed to get the clubcard from payment ###########

        // when booking placed change used to balance booking
        public int Change { get; set; }

        public int PrintNumber { get; set; }
        //
        public ErrorTypes ErrorType { get; set; }

        public int ReprintReason { get; set; }

        /// <summary>
        /// Gets the error description.
        /// </summary>
        /// <value>The error description.</value>
        public string ErrorDescription
        {
            get
            {
                switch (ErrorType)
                {
                    case ErrorTypes.CancelPayment:
                        return "An error occurred whilst cancelling the payment(s)";
                        break;
                    case ErrorTypes.CancelToken:
                        return "An error occurred whilst cancelling the token(s)";
                        break;
                    case ErrorTypes.CreateDotComToken:
                        return "An error occurred whilst creating the Online token(s)";
                        break;
                    case ErrorTypes.CreateToken:
                        return "An error occurred whilst creating the in-store token(s)";
                        break;
                    case ErrorTypes.CreateTokenForClubcard:
                        return "The Clubcard number entered has not been recognised.  Please check that the correct Clubcard number was entered. If it is still not accepted please ask the customer whether they have an alternative Clubcard or ask them to call the Clubcard helpline.";
                        break;
                    case ErrorTypes.InsertBooking:
                        return "An error occurred whilst saving the booking";
                        break;
                    case ErrorTypes.InsertChangePayment:
                        return "An error occurred whilst saving the change payments";
                        break;
                    case ErrorTypes.InsertPrint:
                        return "An error occurred whilst saving the print";
                        break;
                    case ErrorTypes.InsertProductLine:
                        return "An error occurred whilst saving the productline(s)";
                        break;
                    case ErrorTypes.InsertVoucher:
                        return "An error occurred whilst saving the voucher(s)";
                        break;
                    case ErrorTypes.InvalidPaymentResponse:
                        return "An error occurred whilst checking/processing the voucher(s)";
                        break;
                    case ErrorTypes.InvalidTokenResponse:
                        return "An error occurred whilst requesting the token(s)";
                        break;
                    case ErrorTypes.ProcessPayment:
                        return "An error occurred whist processing the payments";
                        break;
                    case ErrorTypes.UpdateBooking:
                        return "An error occurred whilst updating the booking";
                        break;
                    case ErrorTypes.UpdateVoucher:
                        return "An error occurred whist updating the voucher(s)";
                        break;
                    case ErrorTypes.BookingProcessError:
                        return "An error occurred whist processing the booking";
                        break;

                    default:
                        return "An error occurred";
                        break;
                }
            }
        }

        public bool TrainingMode { get; set; }

        // for reprint
        public DateTime CreateDate { get; set; }
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }

        /// <summary>
        /// Gets the status description.
        /// </summary>
        /// <value>The status description.</value>
        public string StatusDescription
        {
            get {
                switch (Status)
                {
                    case "B":
                        return "Completed Booking";
                        break;
                    case "O":
                        return "Open Booking";
                        break;
                    case "C":
                        return "Cancelled Booking";
                        break;
                    case "U":   // user initiated cancel
                        return "Cancelled Booking";
                        break;

                    default:
                        return Status;
                }
            
            }

        }
        


        /// <summary>
        /// Initializes a new instance of the <see cref="Booking"/> class.
        /// </summary>
        public Booking()
        {
            Vouchers = new VoucherCollection();

            ProductLines = new ProductLineCollection();

            PrintNumber = 1;
            TrainingMode = false;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Booking"/> class.
        /// </summary>
        /// <param name="hasInstoreProducts">if set to <c>true</c> [has instore products].</param>
        public Booking(bool isInstoreBooking)
        {
            Vouchers = new VoucherCollection();
            ProductLines = new ProductLineCollection();

            PrintNumber = 1;
            TrainingMode = false;

        }


        /// <summary>
        /// Gets the booking balance (in clubcard points).
        /// </summary>
        /// <returns></returns>
        public int GetBookingBalance()
        {
            // all vouchers - product cost - Change
            return Vouchers.GetTotal(false) - ProductLines.GetTotal() - Change;
        }

        /// <summary>
        /// Adds the change to booking.
        /// </summary>
        public void AddChangeToBooking()
        {
            // change is set if more payment than product
            // set to zero if less than payment or no product

            // assign to variable - to reduce having go go through arrays multiple times
            int paymentValue = Vouchers.GetTotal(true);
            int productValue = ProductLines.GetTotal();

            this.Change = (((paymentValue - productValue) > 0) &&
                            (productValue != 0)) ?
                            paymentValue - productValue : 0;

        }



        /// <summary>
        /// Determines whether [has valid clubcard].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has valid clubcard]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasValidClubcard()
        {

            return cClubcard.HasValidClubcard(this.Clubcard);
        }

        /// <summary>
        /// a futher check that a clubcard is not in 
        /// a range of TPF that are not clubcard
        /// </summary>
        /// <returns></returns>
        public bool isValidClubcardTPF()
        {

            return cClubcard.isValidClubcardTPF(this.Clubcard);
        }

        /// <summary>
        /// Processes the booking.
        /// </summary>
        /// <returns>Boolean to indicate whether the processing was successful</returns>
        /// County Code added by seema
        public Boolean ProcessBooking(string country)
        {
            // process the booking
            // reserve, redeem voucher
            // create coupon
            // do any reversals.....
            // although return true / false
            // set the error code for the object and error description

            try
            {
#if (DEBUG)
                this.SaveToTraining("Booking.ProcessBooking start");
#endif

                /// 
                this.BookingId = SaveToBookingTable("O");      // save as open booking
                if (this.BookingId == 0)
                {
                    // error creating
                    return false;   // leave immediately no need to back out
                }
#if (DEBUG)
                this.SaveToTraining("Booking.ProcessBooking before this.ProductLines.Save");
#endif
                // save the productline
                this.ProductLines.Save(this.BookingId);

#if (DEBUG)
                this.SaveToTraining("Booking.ProcessBooking before this.Vouchers.Save");
#endif
                // save the vouchers to be processed
                this.Vouchers.Save(this.BookingId);

                // processing payment - a failure of this may require reversing
                try
                {

#if (DEBUG)
                    this.SaveToTraining("Booking.ProcessBooking before this.Vouchers.ProcessPayment");
#endif
                    //Country Code added by seema
                    this.Vouchers.ProcessPayment(this.UserId, country);

                    // save the vouchers processed (just updating the status)
                    this.Vouchers.Save(this.BookingId);

                    // payment successful so now create the coupons
                    // tokens gets saved during the create  
                    try
                    {
                        foreach (ProductLine productLine in ProductLines)
                        {
#if (DEBUG)
                            this.SaveToTraining("Booking.ProcessBooking before productLine.CreateTokens");
#endif
                            //Country Code added by seema
                            productLine.CreateTokens(BookingId, this.Clubcard, productLine, this.UserId, country);
                        }

                        this.SaveChangePayment();

                        // TODO Enum Status
                        // save the status - update as booking set
                        this.SaveToBookingTable("B");
                        return true;

                    }
                    catch (BookingException ex)
                    {

#if (DEBUG)
                        this.SaveToTraining("Booking.ProcessBooking productline bookingexception before this.CancelBooking(C)");
#endif
                        // cancel vouchers and reverse vouchers required
                        //Country Code added by seema
                        this.CancelBooking("C",country);
                        throw;
                    }
                    catch (Exception ex)
                    {

#if (DEBUG)
                        this.SaveToTraining("Booking.ProcessBooking productline exception before this.CancelBooking(C)");
#endif
                        // cancel vouchers and reverse vouchers required
                        //Country Code added by seema
                        this.CancelBooking("C",country);
                        throw;
                    }
                }
                catch (BookingException ex)
                {

#if (DEBUG)
                    this.SaveToTraining("Booking.ProcessBooking productline bookingexception payment or cancel");
#endif
                    // save that booking is to be cancelled
                    this.SaveToBookingTable("C");
                    //Country Code added by seema
                    this.Vouchers.CancelPayments(this.BookingId, this.UserId,country);
                    throw;
                }
                catch (Exception ex)
                {
#if (DEBUG)
                    this.SaveToTraining("Booking.ProcessBooking productline exception payment or cancel");
#endif
                    // save that booking is to be cancelled
                    this.SaveToBookingTable("C");
                    throw;
                }
            }
            catch (BookingException ex)
            {
#if (DEBUG)
                this.SaveToTraining("Booking.ProcessBooking productline bookingexception outer catch");
#endif
                this.ErrorType = ex.ErrorType;

                this.SaveToBookingError(ex.ToString());
                return false;
            }
            catch (Exception ex)
            {
#if (DEBUG)
                this.SaveToTraining("Booking.ProcessBooking productline exception outer catch");
#endif

                this.SaveToBookingError(ex.ToString());

                this.ErrorType = ErrorTypes.BookingProcessError;

                return false;
            }
#if (DEBUG)
            this.SaveToTraining("Booking.ProcessBooking unreachable return false");
#endif
            return false;
        }


        /// <summary>
        /// Saves to booking table. Public for testing.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public int SaveToBookingTable(string status)
        {

            int bookingid;
            // should only be called when not set - check just in case
            if ((this.BookingId == 0) || (status == "O" && this.BookingId != 0))
            {
                try
                {
                    bookingid = 0;
                    InstoreClubcardReward.Data.InsertBooking ib = new InstoreClubcardReward.Data.InsertBooking(ConnectionString);
                    ib.Clubcard = this.Clubcard;
                    ib.Status = status;
                    ib.StoreId = this.StoreId;
                    ib.UserId = this.UserId;
                    ib.TillId = this.TillId;

                    ib.Execute();

                    bookingid = (int)ib.BookingId;
                }
                catch (Exception ex)
                {
                    throw new BookingException(ErrorTypes.InsertBooking, string.Format("Clubcard {0}", this.Clubcard), ex);
                }

            }
            else
            {
                try
                {
                    // this is an update (just record status changes)
                    InstoreClubcardReward.Data.UpdateBooking ub = new InstoreClubcardReward.Data.UpdateBooking(ConnectionString);
                    ub.BookingId = this.BookingId;
                    ub.Status = status;
                    ub.Execute();

                    // hasn't change but set so that it can be returned
                    bookingid = this.BookingId;
                }
                catch (Exception ex)
                {
                    throw new BookingException(ErrorTypes.UpdateBooking, string.Format("Status {0}", status), ex);
                }
            }
            return bookingid;
        }


        /// <summary>
        /// Saves the change payment.
        /// </summary>
        private void SaveChangePayment()
        {


            try
            {
                // change should be greater than zero for saving (< 0 is an error and should not occur)
                if (Change > 0)
                {
                    InstoreClubcardReward.Data.InsertPaymentChange ipc = new InstoreClubcardReward.Data.InsertPaymentChange(ConnectionString);
                    ipc.Clubcard = this.Clubcard;
                    ipc.BookingId = this.BookingId;
                    ipc.Amount = this.Change;

                    ipc.Execute();

                }
            }
            catch (Exception ex)
            {
                throw new BookingException(ErrorTypes.InsertChangePayment, "", ex);
            }


        }


        /// <summary>
        /// Saves to BookingError table. On save with a booking 
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public void SaveToBookingError(string error)
        {

            if (this.BookingId != 0)
            {
                try
                {
                    InstoreClubcardReward.Data.InsertBookingError ibe = new InstoreClubcardReward.Data.InsertBookingError(ConnectionString);
                    ibe.BookingId = this.BookingId;
                    ibe.ErrorMessage = error;
                    ibe.ErrorDate = DateTime.Now;

                    ibe.Execute();
                }
                catch (Exception ex)
                {
                    // this saves the error trying to be saved
                    EventLog objEventLog = new EventLog("Application");
                    objEventLog.Source = "ICCR";
                    objEventLog.WriteEntry(string.Format("ICCR SaveToBookingError. BookingId {0}, Error: {1}", BookingId, error));

                    // If the saving fails to save the error then supress the error
                }
            }

        }



        /// <summary>
        /// Save the print script.
        /// </summary>
        /// <param name="printParameter">The print parameter.</param>
        /// <param name="reprintReason">The reprint reason.</param>
        /// <returns></returns>
        public bool SavePrintScript(string printParameter, int? reprintReason)
        {
            try
            {
                // Include saving the print
                InstoreClubcardReward.Data.InsertPrintBooking ip = new InstoreClubcardReward.Data.InsertPrintBooking(ConnectionString);
                ip.BookingId = this.BookingId;
                ip.PrintNumber = this.PrintNumber;
                ip.PrintParameter = printParameter;
                ip.PrintDate = DateTime.Now;
                // check to see whether the print reason is set and if so save this.
                if (reprintReason.HasValue)
                {
                    ip.PrintReasonId = reprintReason.Value;
                }
                ip.Execute();

                this.PrintNumber++;     // increment printer for if a reprint is required
            }
            catch (Exception ex)
            {
                throw new BookingException(ErrorTypes.InsertPrint, "", ex);
            }

            return true;
        }


        /// <summary>
        /// Cancels the booking. "C" system cancellation, "U" user cancellation
        /// </summary>
        /// <param name="cancelType"></param>
        /// Country Code Parameter added by seema
        public void CancelBooking(string cancelType,string country)
        {
            try
            {
                // save that booking is cancelled (system cancel is "C", User initiated "U")
                this.SaveToBookingTable(cancelType);

                // cancel payments
                //Country Code added by seema
                if (this.Vouchers.CancelPayments(this.BookingId, this.UserId,country))
                {
                    if (ProductLines != null)
                    {
                        foreach (ProductLine productLine in ProductLines)
                        {
                            if (productLine.Tokens != null)
                            {
                                foreach (Token token in productLine.Tokens)
                                {
                                    // cancel tokens
                                    if (productLine.Tokens.CancelTokens(this.BookingId, this.Clubcard, this.UserId))
                                    {
                                        // successful cancellation 
                                    }
                                    else
                                    {
                                        throw new BookingException(ErrorTypes.CancelToken, "CancelBooking() Booking.cs - Error cancelling tokens");
                                    }
                                }
                            }
                        }
                    }
                    
                }
                else
                {
                    throw new BookingException(ErrorTypes.CancelPayment, "CancelBooking() Booking.cs - Error cancelling payments");
                }
            }
            catch (BookingException ex)
            {
                this.SaveToBookingError(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                this.SaveToBookingError(ex.ToString());
                throw new BookingException(ErrorTypes.CancelBooking, "", ex);
            }



        }



        /// <summary>
        /// Displays Booking as an XML string
        /// </summary>
        /// <returns></returns>
        public string ToXMLString()
        {
            // serialize to string (reuse serializer defined above)
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
            StringBuilder sb = new StringBuilder();
            using (System.IO.StringWriter stringWriter = new System.IO.StringWriter())
            {
                x.Serialize(stringWriter, this);
                sb = stringWriter.GetStringBuilder();
            }

            return sb.ToString();

        }

        /// <summary>
        /// Updates the used vouchers.
        /// </summary>
        public void UpdateUsedVouchers()
        {
            int balance = this.GetBookingBalance();

            var sortedVouchers =
                from v in this.Vouchers
                orderby v.Value descending
                select v;

            foreach (Voucher v in sortedVouchers)
            {

                if (v.Value <= balance)
                {
                    int voucherIndex = Vouchers.GetVoucherIndex(v.Ean);
                    // If the voucher was found then update the voucher to indicate it's used
                    if (voucherIndex > -1)
                    {
                        Vouchers[voucherIndex].IsUsed = false;
                    }

                    balance = balance - v.Value;
                }
                else
                {
                    int voucherIndex = Vouchers.GetVoucherIndex(v.Ean);
                    // If the voucher was found then update the voucher to indicate it's used
                    if (voucherIndex > -1)
                    {
                        Vouchers[voucherIndex].IsUsed = true;
                    }

                }
            }

        }

        /// <summary>
        /// Gets the till coupon script.
        /// </summary>
        /// <returns></returns>
        public string GetTillTokenScript(int? reprintReason)
        {
            StringBuilder tokenScript = new StringBuilder();

            bool firstTimeThrough = true;
            string barcode = string.Empty;

#if (DEBUG)
            this.SaveToTraining("Booking.GetTillTokenScript before looping products");
#endif

            foreach (ProductLine productLine in ProductLines)
            {

#if (DEBUG)
                this.SaveToTraining("Booking.GetTillTokenScript before looping tokens");
#endif                
                // Loop through the tokens 
                foreach (var token in productLine.Tokens)
                {
                    if (token.GetType() != typeof(DotcomToken))
                    {
#if (DEBUG)
                        this.SaveToTraining("Booking.GetTillTokenScript Not DotCom token: " + token.TokenId);
#endif  

                        //Check that all elements are populated
                        if (string.IsNullOrEmpty(token.ProductCode) ||
                            string.IsNullOrEmpty(token.EAN) ||
                            token.TokenId == 0 ||
                            this.StoreId == 0 ||
                            this.TillId == 0)
                        {
                            SaveToBookingError(string.Format("GetTillTokenScript() Booking.cs Line 649 - Some elements were not populated - Product Code:{0} EAN:{1} TokenId:{2} StoreId:{3} TillId:{4}",
                                token.ProductCode,
                                token.EAN,
                                token.TokenId,
                                this.StoreId,
                                this.TillId));

                            throw new BookingException(ErrorTypes.CreateTillCoupons, "GetTillTokenScript() Booking.cs Line 649 - Some elements were not populated");
                        }
                        else
                        {
                            barcode = token.EAN;
                        }
                    }
                    else
                    {
#if (DEBUG)
                        this.SaveToTraining("Booking.GetTillTokenScript DotCom token: " + token.TokenId);
#endif  

                        //Check that all elements are populated
                        if (string.IsNullOrEmpty(token.ProductCode) ||
                            string.IsNullOrEmpty(token.Alpha) ||
                            token.TokenId == 0 ||
                            this.StoreId == 0 ||
                            this.TillId == 0)
                        {
                            SaveToBookingError(string.Format("GetTillTokenScript() Booking.cs Line 649 - Some elements were not populated - Product Code:{0} Alpha:{1} TokenId:{2} StoreId:{3} TillId:{4}",
                                token.ProductCode,
                                token.Alpha,
                                token.TokenId,
                                this.StoreId,
                                this.TillId));

                            throw new BookingException(ErrorTypes.CreateTillCoupons, "GetTillTokenScript() Booking.cs Line 649 - Some elements were not populated");
                        }
                        else
                        {
                            barcode = token.Alpha;
                        }
                    }

                    if (!firstTimeThrough)
                    {
                        tokenScript.Append("~");
                    }

                    try
                    {

#if (DEBUG)
                        this.SaveToTraining("Booking.GetTillTokenScript before collating tokenScript for token: " + token.TokenId);
#endif  
                        tokenScript.AppendFormat("{0}^{1}^{2}^{3}^{4}^{5}^{6}",
                            token.ProductCode, // Product Type
                            barcode, // Barcode
                            (int)productLine.Product.TokenType, // tokenType (1 in store, 2 direct, 3 online)
                            token.GetCustomerDate(), // Expiry Date                    
                            string.Format("{0};{1};{2}",
                                token.TokenId,
                                this.StoreId,
                                this.TillId
                                ), // footer on coupon
                            token.GetTokenCustomerValue().ToString(currencyFormat), // token value                   
                            Products.GetTokenDescription(token.ProductCode)

                            );
#if (DEBUG)
                        this.SaveToTraining("Booking.GetTillTokenScript after collating tokenScript for token: " + token.TokenId);
#endif  

                    }
                    catch (Exception ex)
                    {
                        SaveToBookingError(string.Format("GetTillTokenScript() Booking.cs Line 736 - Token script creating failed - Product Code:{0} EAN:{1} TokenId:{2} Exception:{3}",
                            token.ProductCode,
                            token.EAN,
                            token.TokenId,
                            ex.ToString()));
                        throw new BookingException(ErrorTypes.CreateTillCoupons, "GetTillTokenScript() Booking.cs Line 736 " + ex.ToString());
                    }

                    if (firstTimeThrough)
                    {
                        firstTimeThrough = false;
                    }

                }

            }

            try
            {

#if (DEBUG)
                this.SaveToTraining("Booking.GetTillTokenScript before getting receipt script");
#endif 
                // add the receipt script
                tokenScript.AppendFormat("~{0}", GetExchangeTillTokenScript());
#if (DEBUG)
                this.SaveToTraining("Booking.GetTillTokenScript after getting receipt script");
#endif 

            }
            catch (Exception ex)
            {
                throw new BookingException(ErrorTypes.CreateTillCoupons, "GetTillTokenScript() wrapper GetExchangeTillTokenScript",ex);                    
            }

            try
            {
#if (DEBUG)
                this.SaveToTraining("Booking.GetTillTokenScript before saving total till script");
#endif 
                // Save the script to the database
                this.SavePrintScript(tokenScript.ToString(), reprintReason);
#if (DEBUG)
                this.SaveToTraining("Booking.GetTillTokenScript after saving total till script");
#endif 
            }
            catch (Exception ex)
            {
#if (DEBUG)
                this.SaveToTraining(String.Format("Booking.GetTillTokenScript error occurred during saving till script:{0}", ex));
#endif 
                throw new BookingException(ErrorTypes.CreateTillCoupons, "GetTillTokenScript() save script to database ", ex);
            }

            // return the script generated
            return tokenScript.ToString();

        }

        /// <summary>
        /// Gets the till coupon script.
        /// </summary>
        /// <returns></returns>
        /// Added by Dimple to make a WcfCall
        public string GetTillTokenScriptWCF(int? reprintReason)
        {
            StringBuilder tokenScript = new StringBuilder();

            bool firstTimeThrough = true;
            string barcode = string.Empty;

#if (DEBUG)
            this.SaveToTraining("Booking.GetTillTokenScript before looping products");
#endif

            foreach (ProductLine productLine in ProductLines)
            {

#if (DEBUG)
                this.SaveToTraining("Booking.GetTillTokenScript before looping tokens");
#endif
                // Loop through the tokens 
                foreach (var token in productLine.Tokens)
                {
                    if (productLine.Product.TokenType == TokenType.Token)
                    {
#if (DEBUG)
                        this.SaveToTraining("Booking.GetTillTokenScript Not DotCom token: " + token.TokenId);
#endif

                        //Check that all elements are populated
                        if (string.IsNullOrEmpty(token.ProductCode) ||
                            string.IsNullOrEmpty(token.EAN) ||
                            token.TokenId == 0 ||
                            this.StoreId == 0 ||
                            this.TillId == 0)
                        {
                            SaveToBookingError(string.Format("GetTillTokenScript() Booking.cs Line 649 - Some elements were not populated - Product Code:{0} EAN:{1} TokenId:{2} StoreId:{3} TillId:{4}",
                                token.ProductCode,
                                token.EAN,
                                token.TokenId,
                                this.StoreId,
                                this.TillId));

                            throw new BookingException(ErrorTypes.CreateTillCoupons, "GetTillTokenScript() Booking.cs Line 649 - Some elements were not populated");
                        }
                        else
                        {
                            barcode = token.EAN;
                        }
                    }
                    else
                    {
#if (DEBUG)
                        this.SaveToTraining("Booking.GetTillTokenScript DotCom token: " + token.TokenId);
#endif

                        //Check that all elements are populated
                        if (string.IsNullOrEmpty(token.ProductCode) ||
                            string.IsNullOrEmpty(token.Alpha) ||
                            token.TokenId == 0 ||
                            this.StoreId == 0 ||
                            this.TillId == 0)
                        {
                            SaveToBookingError(string.Format("GetTillTokenScript() Booking.cs Line 649 - Some elements were not populated - Product Code:{0} Alpha:{1} TokenId:{2} StoreId:{3} TillId:{4}",
                                token.ProductCode,
                                token.Alpha,
                                token.TokenId,
                                this.StoreId,
                                this.TillId));

                            throw new BookingException(ErrorTypes.CreateTillCoupons, "GetTillTokenScript() Booking.cs Line 649 - Some elements were not populated");
                        }
                        else
                        {
                            barcode = token.Alpha;
                        }
                    }

                    if (!firstTimeThrough)
                    {
                        tokenScript.Append("~");
                    }

                    try
                    {

#if (DEBUG)
                        this.SaveToTraining("Booking.GetTillTokenScript before collating tokenScript for token: " + token.TokenId);
#endif
                        tokenScript.AppendFormat("{0}^{1}^{2}^{3}^{4}^{5}^{6}",
                            token.ProductCode, // Product Type
                            barcode, // Barcode
                            (int)productLine.Product.TokenType, // tokenType (1 in store, 2 direct, 3 online)
                            token.GetCustomerDate(), // Expiry Date                    
                            string.Format("{0};{1};{2}",
                                token.TokenId,
                                this.StoreId,
                                this.TillId
                                ), // footer on coupon
                            token.GetTokenCustomerValue().ToString(currencyFormat), // token value                   
                            Products.GetTokenDescriptionWCF(token.ProductCode)

                            );
#if (DEBUG)
                        this.SaveToTraining("Booking.GetTillTokenScript after collating tokenScript for token: " + token.TokenId);
#endif

                    }
                    catch (Exception ex)
                    {
                        SaveToBookingError(string.Format("GetTillTokenScript() Booking.cs Line 736 - Token script creating failed - Product Code:{0} EAN:{1} TokenId:{2} Exception:{3}",
                            token.ProductCode,
                            token.EAN,
                            token.TokenId,
                            ex.ToString()));
                        throw new BookingException(ErrorTypes.CreateTillCoupons, "GetTillTokenScript() Booking.cs Line 736 " + ex.ToString());
                    }

                    if (firstTimeThrough)
                    {
                        firstTimeThrough = false;
                    }

                }

            }

            try
            {

#if (DEBUG)
                this.SaveToTraining("Booking.GetTillTokenScript before getting receipt script");
#endif
                // add the receipt script
                tokenScript.AppendFormat("~{0}", GetExchangeTillTokenScript());
#if (DEBUG)
                this.SaveToTraining("Booking.GetTillTokenScript after getting receipt script");
#endif

            }
            catch (Exception ex)
            {
                throw new BookingException(ErrorTypes.CreateTillCoupons, "GetTillTokenScript() wrapper GetExchangeTillTokenScript", ex);
            }

            try
            {
#if (DEBUG)
                this.SaveToTraining("Booking.GetTillTokenScript before saving total till script");
#endif
                // Save the script to the database
                this.SavePrintScript(tokenScript.ToString(), reprintReason);
#if (DEBUG)
                this.SaveToTraining("Booking.GetTillTokenScript after saving total till script");
#endif
            }
            catch (Exception ex)
            {
#if (DEBUG)
                this.SaveToTraining(String.Format("Booking.GetTillTokenScript error occurred during saving till script:{0}", ex));
#endif
                throw new BookingException(ErrorTypes.CreateTillCoupons, "GetTillTokenScript() save script to database ", ex);
            }

            // return the script generated
            return tokenScript.ToString();

        }


        /// <summary>
        /// Added by Vijeth For Exchange Receipt
        /// Gets the Exchange till coupon script.
        /// </summary>
        /// <returns></returns>
        // checks producing token will have thrown errors so no 
        // need to do checks
        public string GetTillReceiptScript()
        {
            StringBuilder output = new StringBuilder();

            // Start Receipt
            output.Append("//header///Reward Tokens received//");

            // Display Reward Tokens received
            // Loop through product / token
            // Loop through the tokens 
            string productCode = string.Empty; ;

            foreach (ProductLine productLine in ProductLines)
            {
                foreach (var token in productLine.Tokens)
                {
                    if (productCode != token.ProductCode)
                    {
                        // product description

                        // record so only done one per product
                        productCode = token.ProductCode;
                        // use LinQ
                        Product chosenProduct = Products.First(p => p.ProductCode == productCode);

                        output.Append(chosenProduct.Description);


                    }
                    if (token.GetType() != typeof(DotcomToken))
                    {
                        // number
                        output.Append(token.EAN);
                    }
                    else
                    {
                        // alpha
                        output.Append(token.Alpha);
                    }

                    output.Append("    £10.00");
                }

            }
            


            output.Append("//blank line//");

            // display Total Reward value 
            output.Append("Total Reward value");
            output.Append(((this.ProductLines.GetTotal() * 2) / 100f).ToString("c"));


            // display Total Reward tokens
            output.Append("Total Reward tokens");
            output.Append(this.GetTokenCount().ToString());


            output.Append("//blank line//");


            // Display Clubcard vouchers exchanged
            // Loop through vouchers
            foreach (Voucher cv in Vouchers)
            {
                if (cv.IsUsed)
                {
                    output.Append(cv.Ean);
                    output.Append((cv.Value / 100f).ToString("c"));
                }
            }

            output.Append("//blank line//");

            // Display Total voucher value 
            output.Append("Total voucher value");
            output.Append((Vouchers.GetTotal(true) / 100f).ToString("c"));

            // Display Value used  
            output.Append("Value used");
            output.Append(((this.ProductLines.GetTotal()) / 100f).ToString("c"));


            output.Append("//blank line//");

            // prepare change


            //Display Clubcard account change   
            output.Append("Clubcard account change");
            output.Append((Change / 100f).ToString("c"));

            // Display Received as Clubcard points
            output.Append("Received as Clubcard points");
            output.Append(Change.ToString("c"));

            // Finish off Receipt
            output.Append("//finish off//");

            return output.ToString();
        }



        /// <summary>
        /// get product number total - same as number of tokens
        /// ###### might change with deals tokens / value
        /// </summary>
        /// <returns></returns>
        private int GetTokenCount()
        {
            // call to productline collection
            return ProductLines.GetProductNumberTotal();
        }



        /// <summary>
        /// get the total token value
        /// </summary>
        /// <returns></returns>
        public int GetTotalTokenValue()
        {
            // call to productline collection
            return ProductLines.GetTotalTokenValue();
        }



        public string GetExchangeTillTokenScript()
        {
            StringBuilder ExchangeReceiptOutput = new StringBuilder();
            StringBuilder VouchersAsOutput = new StringBuilder();
            StringBuilder TokensAsOutput = new StringBuilder();


            // product a string of repeated token ean or alpha , token value as currency, product code %

            foreach (ProductLine productLine in ProductLines)
            {
                foreach (var token in productLine.Tokens)
                {

                    // Description^TokenValue% for each token.....
                    TokensAsOutput.AppendFormat("{0}^{1}%",
                        productLine.Product.TokenTitle,
                        (token.TokenValue / 100.0).ToString(currencyFormat)
                    );
                }
            }

            // token totals
            string TotalTokenValue = (GetTotalTokenValue() / 100.0).ToString(currencyFormat);
            string TotalTokenCount = GetTokenCount().ToString();


            // product a string of repeated #masked ean , voucher value as currency
            foreach (Voucher v in Vouchers)
            {
                VouchersAsOutput.AppendFormat("#{0}^{1}", 
                    "xxxxxxxxxxxxxxxxxx" + v.Ean.Substring(18, 4),
                    (v.Value / 100F).ToString(currencyFormat));
            }

            // add a final # to finish off the record
            VouchersAsOutput.Append("#");


            // total voucher value used (true)
            string TotalVoucherValue = ((Vouchers.GetTotal(true)) / 100F).ToString(currencyFormat);

            // change as value in clubcard currency
            string ChangeValue = (Change / 100f).ToString(currencyFormat);
            // Change as points
            string ChangePoints = this.Change.ToString();


            // Prepare output
            ExchangeReceiptOutput.AppendFormat("{0}{1}^{2}{3}{4}^{5}^{6}^{7}",
                TokensAsOutput, // token Ean/Aplha with Value
                TotalTokenValue, //Total Reward Value
                TotalTokenCount, // Total Reward Tokens count
                VouchersAsOutput, //Clubcard Vouchers EAN with value
                TotalVoucherValue, //Total Clubcard Vouchers Value
                ChangeValue, //Change to be returned to Customer in Pounds
                ChangePoints, //Change to be returned
                string.Format("{0};{1};{2}",
                    this.BookingId,     // this was tokenId, use bookingid as this represents the whoel receipt
                    this.StoreId,
                    this.TillId
                    ) // footer on coupon                                      
                );

            return ExchangeReceiptOutput.ToString();

        }

        /// <summary>
        /// Saves to Training table. Saves text with
        /// current user id. Used to audit training.
        /// </summary>
        /// <param name="page">The status.</param>
        /// <returns></returns>
        public void SaveToTraining(string locationDescription)
        {
            try
            {
                InstoreClubcardReward.Data.InsertTraining it = new InstoreClubcardReward.Data.InsertTraining(ConnectionString);
                it.StoreId = this.StoreId;
                it.TillId = this.TillId;
                it.UserId = this.UserId;
                it.LocationDescription = locationDescription;
                it.TrainingDate = DateTime.Now;
                it.BookingId = this.BookingId;
                it.Execute();

            }
            catch (Exception ex)
            {
                SaveToTraining(String.Format("Failed to save to training table. Error : {0}", ex.ToString()), 0, 0, 0, 0);
            }

        }

        /// <summary>
        /// Saves to Training table. Saves text with
        /// current user id. Used to audit training.
        /// </summary>
        /// <param name="page">The status.</param>
        /// <returns></returns>
        public static void SaveToTraining(string locationDescription, int storeId, int tillId, int userId, int bookingId)
        {
            try
            {
                InstoreClubcardReward.Data.InsertTraining it = new InstoreClubcardReward.Data.InsertTraining(ConnectionString);
                it.StoreId = storeId;
                it.TillId = tillId;
                it.UserId = userId;
                it.LocationDescription = locationDescription;
                it.TrainingDate = DateTime.Now;
                it.BookingId = bookingId;
                it.Execute();

            }
            catch (Exception ex)
            {
                // just audit information for training so do not raise as an error
            }

        }




        /// <summary>
        /// Gets the booking - used for reprint etc
        /// </summary>
        /// <param name="bookingId">The booking id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public static Booking GetBooking(int bookingId, int userId)
        {
            Collection<SelectBookingByBookingIdRow> bookings = null;
            try
            {
                SelectBookingByBookingId bookingsByBookingId = new SelectBookingByBookingId(ConnectionString);
                bookingsByBookingId.BookingId = bookingId;
                bookingsByBookingId.AgentId = userId;
                bookings = bookingsByBookingId.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving booking", ex);

            }

            if (bookings.Count > 0)
            {

                try
                {
                    SelectBookingByBookingIdRow bookingRow = bookings[0];

                    // create the new booking
                    Booking booking = new Booking();

                    booking.BookingId = (int)bookingRow.BookingId;
                    booking.CreateDate = (DateTime)bookingRow.CreateDate;
                    booking.Clubcard = (string)bookingRow.Clubcard;
                    booking.Status = (string)bookingRow.Status;
                    booking.StatusDate = (DateTime)bookingRow.StatusDate;
                    booking.StoreId = (int)bookingRow.StoreId;
                    booking.UserId = (int)bookingRow.UserId;
                    booking.TillId = (int)bookingRow.TillId;

                    booking.ProductLines = ProductLineCollection.GetProductLines(bookingId, booking.UserId);
                    //booking.Vouchers = VoucherCollection.GetVouchers(bookingId);
                    //booking.Change = GetChangePayments(bookingId);

                    return booking;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error populating booking", ex);
                }
            }
            else
            {
                return null;
            }



        }

        /// <summary>
        /// Gets the booking - used for reprint etc
        /// </summary>
        /// <param name="bookingId">The booking id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        /// Added by Dimple to make a WcfCall
        public static Booking GetBookingWCF(int bookingId, int userId)
        {
            Collection<SelectBookingByBookingIdRow> bookings = null;
            try
            {
                SelectBookingByBookingId bookingsByBookingId = new SelectBookingByBookingId(ConnectionString);
                bookingsByBookingId.BookingId = bookingId;
                bookingsByBookingId.AgentId = userId;
                bookings = bookingsByBookingId.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving booking", ex);

            }

            if (bookings.Count > 0)
            {

                try
                {
                    SelectBookingByBookingIdRow bookingRow = bookings[0];

                    // create the new booking
                    Booking booking = new Booking();

                    booking.BookingId = (int)bookingRow.BookingId;
                    booking.CreateDate = (DateTime)bookingRow.CreateDate;
                    booking.Clubcard = (string)bookingRow.Clubcard;
                    booking.Status = (string)bookingRow.Status;
                    booking.StatusDate = (DateTime)bookingRow.StatusDate;
                    booking.StoreId = (int)bookingRow.StoreId;
                    booking.UserId = (int)bookingRow.UserId;
                    booking.TillId = (int)bookingRow.TillId;

                    booking.ProductLines = ProductLineCollection.GetProductLinesWCF(bookingId, booking.UserId);
                    //booking.Vouchers = VoucherCollection.GetVouchers(bookingId);
                    //booking.Change = GetChangePayments(bookingId);

                    return booking;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error populating booking", ex);
                }
            }
            else
            {
                return null;
            }



        }

        /// <summary>
        /// Gets the bookings for clubcard.
        /// </summary>
        /// <param name="clubcard">The clubcard.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public static List<Booking> GetBookingsForClubcard(string clubcard, DateTime startDate, DateTime endDate, int userId)
        {
            //check the clubcard is populated before continuing
            if (String.IsNullOrEmpty(clubcard))
                throw new ArgumentException("clubcard is null or empty.", "clubcard");

            List<Booking> bookings = new List<Booking>();

            Collection<SelectBookingsByClubcardRow> bookingsForClubcard;

            try
            {
                SelectBookingsByClubcard selectBookingsByClubcard = new SelectBookingsByClubcard(ConnectionString);
                selectBookingsByClubcard.Clubcard = clubcard;
                selectBookingsByClubcard.Start = startDate;
                selectBookingsByClubcard.End = endDate;
                bookingsForClubcard = selectBookingsByClubcard.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving bookings for the clubcard", ex);
            }

            // check to see whether we have any bookings for the clubcard
            if (bookingsForClubcard.Count > 0)
            {
                int bookingId;

                // loop through the bookings and populate the booking objects 
                foreach (SelectBookingsByClubcardRow bookingForClubcard in bookingsForClubcard)
                {
                    try
                    
                    
                    {
                        bookingId = int.Parse(bookingForClubcard.BookingId.ToString());

                        bookings.Add(GetBooking(bookingId, userId));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error populating bookings for the clubcard", ex);
                    }
                }
            }

            return bookings;
        }

        /// <summary>
        /// Gets the bookings for clubcard.
        /// </summary>
        /// <param name="clubcard">The clubcard.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        /// Added by Dimple to make a WcfCall
        public static List<Booking> GetBookingsForClubcardWCF(string clubcard, DateTime startDate, DateTime endDate, int userId)
        {
            //check the clubcard is populated before continuing
            if (String.IsNullOrEmpty(clubcard))
                throw new ArgumentException("clubcard is null or empty.", "clubcard");

            List<Booking> bookings = new List<Booking>();

            Collection<SelectBookingsByClubcardRow> bookingsForClubcard;

            try
            {
                SelectBookingsByClubcard selectBookingsByClubcard = new SelectBookingsByClubcard(ConnectionString);
                selectBookingsByClubcard.Clubcard = clubcard;
                selectBookingsByClubcard.Start = startDate;
                selectBookingsByClubcard.End = endDate;
                bookingsForClubcard = selectBookingsByClubcard.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving bookings for the clubcard", ex);
            }

            // check to see whether we have any bookings for the clubcard
            if (bookingsForClubcard.Count > 0)
            {
                int bookingId;

                // loop through the bookings and populate the booking objects 
                foreach (SelectBookingsByClubcardRow bookingForClubcard in bookingsForClubcard)
                {
                    try
                    {
                        bookingId = int.Parse(bookingForClubcard.BookingId.ToString());

                        bookings.Add(GetBookingWCF(bookingId, userId));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error populating bookings for the clubcard", ex);
                    }
                }
            }

            return bookings;
        }


        /// <summary>
        /// Gets the reprint till token script.
        /// </summary>
        /// <param name="bookingId">The booking id.</param>
        /// <returns></returns>
        public string GetReprintTillTokenScript(int bookingId, int? reprintReason)
        {

            try
            {

                SelectPrintsByBookingId selectPrintsByBookingId = new SelectPrintsByBookingId(ConnectionString);
                selectPrintsByBookingId.BookingId = bookingId;
                Collection<SelectPrintsByBookingIdRow> printRows = selectPrintsByBookingId.Execute();

                if (printRows.Count > 0)
                {
                    // return PrintParameter field - with any reprint adjustment
                    string reprintParameter = printRows[printRows.Count - 1].PrintParameter.ToString();
                    PrintNumber = printRows[printRows.Count - 1].PrintNumber.Value + 1;
                    SavePrintScript(reprintParameter, reprintReason);
                    return reprintParameter;

                }
                else
                {
                    PopulateBooking(bookingId);

                    string tillTokenScript = GetTillTokenScript(reprintReason);
                    if (!string.IsNullOrEmpty(tillTokenScript))
                    {
                        return tillTokenScript;
                    }
                    else
                    {
                        throw new BookingException(ErrorTypes.CreateTillCoupons, "GetReprintTillTokenScript() Booking.cs - Error retrieving print script ");
                    }
                }
            }
            catch (Exception ex)
            {
                SaveToBookingError(string.Format("GetReprintTillTokenScript() Booking.cs - Error retrieving print script BookingId:{0} Error:{1}",
                            bookingId,                            
                            ex.ToString()));
                throw new BookingException(ErrorTypes.CreateTillCoupons, "GetReprintTillTokenScript() Booking.cs - Error retrieving print script " + ex.ToString());
            }

        }



        /// Added by Dimple to make a WcfCall
        public string GetReprintTillTokenScriptWCF(int bookingId, int? reprintReason)
        {

            try
            {

                SelectPrintsByBookingId selectPrintsByBookingId = new SelectPrintsByBookingId(ConnectionString);
                selectPrintsByBookingId.BookingId = bookingId;
                Collection<SelectPrintsByBookingIdRow> printRows = selectPrintsByBookingId.Execute();

                if (printRows.Count > 0)
                {
                    // return PrintParameter field - with any reprint adjustment
                    string reprintParameter = printRows[printRows.Count - 1].PrintParameter.ToString();
                    PrintNumber = printRows[printRows.Count - 1].PrintNumber.Value + 1;
                    SavePrintScript(reprintParameter, reprintReason);
                    return reprintParameter;

                }
                else
                {
                    PopulateBookingWCF(bookingId);

                    string tillTokenScript = GetTillTokenScriptWCF(reprintReason);
                    if (!string.IsNullOrEmpty(tillTokenScript))
                    {
                        return tillTokenScript;
                    }
                    else
                    {
                        throw new BookingException(ErrorTypes.CreateTillCoupons, "GetReprintTillTokenScript() Booking.cs - Error retrieving print script ");
                    }
                }
            }
            catch (Exception ex)
            {
                SaveToBookingError(string.Format("GetReprintTillTokenScript() Booking.cs - Error retrieving print script BookingId:{0} Error:{1}",
                            bookingId,
                            ex.ToString()));
                throw new BookingException(ErrorTypes.CreateTillCoupons, "GetReprintTillTokenScript() Booking.cs - Error retrieving print script " + ex.ToString());
            }

        }


        /// <summary>
        /// Add to the original print script to indicate reprint
        /// </summary>
        /// <param name="reprint"></param>
        /// <returns></returns>
        private string addReprintInfo(string reprint)
        {
            //TODO #########################
            // Add "R" to reprint script
            // just a small mark on final parameter.....

            // this will rely on knowing the format of the message
            // per token this is every 3rd ^
            // for receipt this is 7th ^
            return reprint;

        }

        public void PopulateBooking(int bookingId)
        {
            try
            {
                SelectBookingByBookingId selectBookingByBookingId = new SelectBookingByBookingId(ConnectionString);
                selectBookingByBookingId.BookingId = bookingId;
                Collection<SelectBookingByBookingIdRow> bookings = selectBookingByBookingId.Execute();

                if (bookings.Count > 0)
                {
                    this.BookingId = bookingId;
                    this.Clubcard = bookings[0].Clubcard;
                    this.CreateDate = bookings[0].CreateDate.Value;
                    this.ProductLines = PopulateProductLines(bookingId);
                    this.Products = ProductCollection.GetProducts();
                    this.Status = bookings[0].Status;
                    this.StatusDate = bookings[0].StatusDate.Value;
                    this.StoreId = bookings[0].StoreId.Value;
                    this.TillId = bookings[0].TillId.Value;
                    this.Vouchers = PopulateVouchers(bookingId);
                    this.Change = PopulateChange(bookingId);

                }
            }
            catch (Exception ex)
            {
                SaveToBookingError(string.Format("PopulateBooking() Booking.cs - Error populating booking BookingId:{0} Error:{1}",
                           bookingId,
                           ex.ToString()));                
            }

        }

        /// Added by Dimple to make a WcfCall
        public void PopulateBookingWCF(int bookingId)
        {
            try
            {
                SelectBookingByBookingId selectBookingByBookingId = new SelectBookingByBookingId(ConnectionString);
                selectBookingByBookingId.BookingId = bookingId;
                Collection<SelectBookingByBookingIdRow> bookings = selectBookingByBookingId.Execute();

                if (bookings.Count > 0)
                {
                    this.BookingId = bookingId;
                    this.Clubcard = bookings[0].Clubcard;
                    this.CreateDate = bookings[0].CreateDate.Value;
                    this.ProductLines = PopulateProductLinesWCF(bookingId);
                    this.Products = ProductCollection.GetProductsWCF();
                    this.Status = bookings[0].Status;
                    this.StatusDate = bookings[0].StatusDate.Value;
                    this.StoreId = bookings[0].StoreId.Value;
                    this.TillId = bookings[0].TillId.Value;
                    this.Vouchers = PopulateVouchers(bookingId);
                    this.Change = PopulateChange(bookingId);

                }
            }
            catch (Exception ex)
            {
                SaveToBookingError(string.Format("PopulateBookingWCF() Booking.cs - Error populating booking BookingId:{0} Error:{1}",
                           bookingId,
                           ex.ToString()));
            }

        }

        private ProductLineCollection PopulateProductLines(int bookingId)
        {
            ProductLineCollection productLineCollection = new ProductLineCollection();

            SelectProductLinesByBookingId selectProductLinesByBookingId = new SelectProductLinesByBookingId(ConnectionString);
            selectProductLinesByBookingId.BookingId = bookingId;
            Collection<SelectProductLinesByBookingIdRow> productLines = selectProductLinesByBookingId.Execute();

            SelectTokensByBookingId selectTokensByBookingId = new SelectTokensByBookingId(ConnectionString);
            selectTokensByBookingId.BookingId = bookingId;
            Collection<SelectTokensByBookingIdRow> tokensByBooking = selectTokensByBookingId.Execute();

            SelectSupplierTokenCodesByBookingId selectSupplierTokenCodesByBookingId = new SelectSupplierTokenCodesByBookingId(ConnectionString);
            selectSupplierTokenCodesByBookingId.BookingId = bookingId;
            Collection<SelectSupplierTokenCodesByBookingIdRow> tokenCodesByBooking = selectSupplierTokenCodesByBookingId.Execute();

            foreach (SelectProductLinesByBookingIdRow productLine in productLines)
            {

                ProductLine pl = new ProductLine();
                pl.ProductLineId = productLine.ProductLineId.Value;
                pl.ProductNumber = productLine.ProductNumber.Value;
                pl.Product = GetProduct(productLine.ProductCode);

                var tokenDetails =
                    from t in tokensByBooking
                    join stc in tokenCodesByBooking on t.TokenId equals stc.TokenId
                    where stc.SupplierCode == productLine.ProductCode
                    select new { stc.TokenId, stc.SupplierCode, stc.SupplierTokenCode, stc.CustomerDate };

                foreach (var token in tokenDetails)
                {
                    Token t;

                    if (pl.Product.TokenType == TokenType.Token)
                    {
                        t = new Token(productLine.ProductLineId.Value);
                    }
                    else
                    {
                        t = new DotcomToken(productLine.ProductLineId.Value);
                    }

                    t.ProductCode = pl.Product.ProductCode;
                    t.TokenId = token.TokenId.Value;


                    if (pl.Product.TokenType == TokenType.Token)
                    {
                        t.EAN = token.SupplierTokenCode;
                    }
                    else
                    {
                        t.Alpha = token.SupplierTokenCode;
                    }

                    t.TokenValue = pl.Product.TokenValue;

                    //UsedByDate is used to define exactly when the token should expire as used by GetCustomerDate().
                    t.UsedByDate = token.CustomerDate.Value;

                    pl.Tokens.Add(t);


                }
            
                productLineCollection.Add(pl);
            }

            return productLineCollection;


        }

        /// Added by Dimple to make a WcfCall
        private ProductLineCollection PopulateProductLinesWCF(int bookingId)
        {
            ProductLineCollection productLineCollection = new ProductLineCollection();

            SelectProductLinesByBookingId selectProductLinesByBookingId = new SelectProductLinesByBookingId(ConnectionString);
            selectProductLinesByBookingId.BookingId = bookingId;
            Collection<SelectProductLinesByBookingIdRow> productLines = selectProductLinesByBookingId.Execute();

            SelectTokensByBookingId selectTokensByBookingId = new SelectTokensByBookingId(ConnectionString);
            selectTokensByBookingId.BookingId = bookingId;
            Collection<SelectTokensByBookingIdRow> tokensByBooking = selectTokensByBookingId.Execute();

            SelectSupplierTokenCodesByBookingId selectSupplierTokenCodesByBookingId = new SelectSupplierTokenCodesByBookingId(ConnectionString);
            selectSupplierTokenCodesByBookingId.BookingId = bookingId;
            Collection<SelectSupplierTokenCodesByBookingIdRow> tokenCodesByBooking = selectSupplierTokenCodesByBookingId.Execute();

            foreach (SelectProductLinesByBookingIdRow productLine in productLines)
            {

                ProductLine pl = new ProductLine();
                pl.ProductLineId = productLine.ProductLineId.Value;
                pl.ProductNumber = productLine.ProductNumber.Value;
                pl.Product = GetProductWCF(productLine.ProductCode);

                var tokenDetails =
                    from t in tokensByBooking
                    join stc in tokenCodesByBooking on t.TokenId equals stc.TokenId
                    where stc.SupplierCode == productLine.ProductCode
                    select new { stc.TokenId, stc.SupplierCode, stc.SupplierTokenCode, stc.CustomerDate };

                foreach (var token in tokenDetails)
                {
                    Token t;

                    if (pl.Product.TokenType == TokenType.Token)
                    {
                        t = new Token(productLine.ProductLineId.Value);
                    }
                    else
                    {
                        t = new DotcomToken(productLine.ProductLineId.Value);
                    }

                    t.ProductCode = pl.Product.ProductCode;
                    t.TokenId = token.TokenId.Value;


                    if (pl.Product.TokenType == TokenType.Token)
                    {
                        t.EAN = token.SupplierTokenCode;
                    }
                    else
                    {
                        t.Alpha = token.SupplierTokenCode;
                    }

                    t.TokenValue = pl.Product.TokenValue;

                    //UsedByDate is used to define exactly when the token should expire as used by GetCustomerDate().
                    t.UsedByDate = token.CustomerDate.Value;

                    pl.Tokens.Add(t);


                }

                productLineCollection.Add(pl);
            }

            return productLineCollection;


        }

        private VoucherCollection PopulateVouchers(int bookingId)
        {
            VoucherCollection voucherCollection = new VoucherCollection();

            SelectPaymentVouchersByBookingId selectPaymentVouchersByBookingId = new SelectPaymentVouchersByBookingId(ConnectionString);
            selectPaymentVouchersByBookingId.BookingId = bookingId;
            Collection<SelectPaymentVouchersByBookingIdRow> vouchers = selectPaymentVouchersByBookingId.Execute();

            foreach (SelectPaymentVouchersByBookingIdRow voucher in vouchers)
            {
                Voucher v = new Voucher();
                v.Alpha = voucher.Alpha;
                v.Ean = voucher.EAN;

                voucherCollection.Add(v);

            }

            return voucherCollection;


        }

        private int PopulateChange(int bookingId)
        {
            int changeAmount = 0;

            SelectPaymentChangesByBookingId selectPaymentChangesByBookingId = new SelectPaymentChangesByBookingId(ConnectionString);
            selectPaymentChangesByBookingId.BookingId = bookingId;
            Collection<SelectPaymentChangesByBookingIdRow> changes = selectPaymentChangesByBookingId.Execute();

            foreach (SelectPaymentChangesByBookingIdRow change in changes)
            {
                changeAmount = change.Amount.Value;
                break;
            }

            return changeAmount;

        }

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static Product GetProduct(string productCode)
        {
            ProductCollection products = ProductCollection.GetProducts();

            int productIndex = products.GetIndex(productCode);

            Product product = products[productIndex];
            return product;
        }

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        /// Added by Dimple to make a WcfCall
        private static Product GetProductWCF(string productCode)
        {
            ProductCollection products = ProductCollection.GetProductsWCF();

            int productIndex = products.GetIndex(productCode);

            Product product = products[productIndex];
            return product;
        }

    }
}
