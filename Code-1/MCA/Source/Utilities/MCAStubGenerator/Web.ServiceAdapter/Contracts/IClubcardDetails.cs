using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Tesco.ClubcardProducts.MCA.Web.Entities;
using Tesco.ClubcardProducts.MCA.Web.Entities.ClubcardService;
using Tesco.ClubcardProducts.MCA.Web.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Entities.Points;
using Tesco.ClubcardProducts.MCA.Web.Entities.OrderReplacement;
using Tesco.ClubcardProducts.MCA.Web.Entities.ChristmasSaver;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts
{
    public interface IClubcardDetails
    {
        List<HouseholdCustomerDetails> GetHouseHoldCustomers(long CustomerID, string culture);
        List<Clubcard> GetClubcardsCustomer(long CustomerID, string culture);
        List<CustomerActivationDetails> GetCustomerActivationStatusDetails(string dotcomCustomerID, string culture);
        List<CustomerActivationDetails> GetCustActivationDetails(DataSet dsCustomerHouseholdStatus);
       
        List<Offer> GetOffersForCustomer(long customerId, string culture);
        List<PointsSummary> GetPointsSummary(long customerId, int previousOfferId, string culture);
        AccountDetails GetCustomerAccountDetails(long customerId, string culture);
        //List<CustomerMLS_PDF_DownloadDetails> GetMyAccountDetails(long customerID, string culture);  
        bool IsFuelSaverAccountExists(string clubCardId);
        GetFuelBalanceResponse GetFuelSaverPointsBalance(long customerId);
        OrderReplacementStatus GetOrderReplacementExistingStatus(long customerId);
        bool ProcessOrderReplacementRequest(OrderReplacementModel model);
        CustomerTransactions GetTransactionDetailsByCustomerAndOfferID(long CustomerId, int OfferId, bool ShowMerchantFlag); 
		List<Clubcard> GetMyAccountDetails(long customerID, string culture);
        bool IsXmasClubMember(long customerId, string culture);
        List<ChristmasSaverSummary> GetChristmasSaverSummary(long customerID, DateTime startDate, DateTime endDate, string culture);
    }
}
