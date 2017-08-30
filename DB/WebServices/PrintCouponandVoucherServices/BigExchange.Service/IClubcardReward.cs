using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BigExchange.Service
{
    [ServiceContract]
    public interface IClubcardReward
    {


        [OperationContract]
        [FaultContract(typeof(CustomException))]
        ProductCollection GetProducts();

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        CategoryCollection GetCategories();

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        CategoryIncludesCollection GetCategoryIncludes(int categoryId);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        PrintReasonCollection GetPrintReasons();

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        VoucherCollection ValidateVouchers(VoucherCollection voucherColl, string clubcard, int agentId, string country);
        //void ValidateVouchers(VoucherCollection voucherColl,string clubcard, int agentId, string country);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        Booking ProcessBooking(Booking bookingData, string country);
        //Boolean ProcessBooking(Booking bookingData,string country);


        [OperationContract]
        [FaultContract(typeof(CustomException))]
        void SaveToBookingError(Booking bookingData, string error);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        void SaveToTraining(string locationDescription, int storeId, int tillId, int userId, int bookingId);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        List<Booking> GetBookingsForClubcard(string clubcard, DateTime startDate, DateTime endDate, int userId);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        string GetReprintTillTokenScript(Booking bookingData, int bookingId, int? reprintReason);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        string GetTillTokenScript(Booking bookingData, int? reprintReason);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        string KioskEntryPage(string clientIP);




        [OperationContract]
        [FaultContract(typeof(CustomException))]
        string PrintVoucherKioskEntryPage(string clientIP);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        BookingPrintVoucher ProcessVoucherBooking(BookingPrintVoucher bookingPrintVoucher);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        UnusedVoucherCollection GetUnusedVoucherDetails(string clubcard);
        

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        void Logging(BookingPrintVoucher bookingPrintVoucher, string error, LoggingOperations loggingOperations);

    }      
        
}
