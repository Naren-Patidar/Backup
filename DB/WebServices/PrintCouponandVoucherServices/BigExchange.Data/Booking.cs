using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace BigExchange
{
    /// <summary>
    /// Created Date : 08/06/2011
    /// Created By: Dimple Kandoliya
    /// Class Name: Booking
    /// </summary>
    [DataContract]
    public class Booking
    {
        [DataMember]
        public int BookingId { get; set; }

        // parameters supplied by the calling URL
        [DataMember]
        public int StoreId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int TillId { get; set; }

        // result of the menu choice. Instore or Direct
        // used to determine the products available
        [DataMember]
        public int BookingType { get; set; }

        //we will need a collection of vouchers
        [DataMember]
        public VoucherCollection Vouchers { get; set; }
        [DataMember]
        public ProductCollection Products { get; set; }
        [DataMember]
        public ProductLineCollection ProductLines { get; set; }

        // from payment get a clubcard number to use
        // requires redeem messages to be done.... select first? select most frequent?
        [DataMember]
        public string Clubcard { get; set; } ///// TBD more needed to get the clubcard from payment ###########

        // when booking placed change used to balance booking
        [DataMember]
        public int Change { get; set; }

        [DataMember]
        public int PrintNumber { get; set; }
        [DataMember]
        public ErrorTypes ErrorType { get; set; }
        [DataMember]
        public int ReprintReason { get; set; }

        /// <summary>
        /// Gets the error description.
        /// </summary>
        /// <value>The error description.</value>
        [DataMember]
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
            private set{}
        }
        [DataMember]
        public bool TrainingMode { get; set; }

        // for reprint
        [DataMember]
        public DateTime CreateDate { get; set; }
        
        [DataMember]
        public string Status { get; set; }
        
        [DataMember]
        public DateTime StatusDate { get; set; }

        /// <summary>
        /// Gets the status description.
        /// </summary>
        /// <value>The status description.</value>
        [DataMember]
        public string StatusDescription
        {
            get
            {
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
            private set { }
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
        
    }
}
