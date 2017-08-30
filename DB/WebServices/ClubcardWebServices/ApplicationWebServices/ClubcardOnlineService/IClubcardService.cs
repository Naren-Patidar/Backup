using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Tesco.com.ClubcardOnlineService
{
    [ServiceContract(Namespace = "http://tesco.com/clubcardonline/datacontract/2010/01")]
    public interface IClubcardService
    {
     
        [OperationContract]
        bool IsXmasClubMember(Int64 CustomerID, string culture, out string errorXml, out string resultXml);

        [OperationContract]
        bool GetChristmasSaverSummary(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool GetMyAccountDetails(Int64 CustomerID, string culture, out string errorXml, out string resultXml);

        [OperationContract]
        bool IsNewOrderReplacementValid(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool AddNewOrderReplacement(string updateXml, string consumer, out string errorXml, out Int64 customerID);

        [OperationContract]
        bool GetHouseholdCustomers(Int64 CustomerID, string culture, out string errorXml, out string resultXml);

        [OperationContract]
        bool GetClubcards(Int64 CustomerID, string culture, out string errorXml, out string resultXml);

        [OperationContract]
        bool GetClubcardsCustomer(Int64 CustomerID, string culture, out string errorXml, out string resultXml);

        [OperationContract]
        bool GetPointsForAllCollPeriodByCustomer(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool GetTxnDetailsByCustomerAndOfferID(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool GetTxnDetailsByHouseholdCustomerAndOfferID(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool GetPointsSummaryInfo(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool CheckCustomerActivated(Int64 dotcomCustomerID, out char activated, out Int64 customerID, string culture, out string errorXml, out string resultXml);

        [OperationContract]
        bool IGHSCheckCustomerActivated(string dotcomCustomerID, out char activated, out Int64 customerID, string culture, out string errorXml, out string resultXml);

        [OperationContract]
        bool CheckHouseholdStatusOfCustomer(Int64 customerID, string culture, out string errorXml, out string resultXml);

        [OperationContract]
        bool AddPrimaryCard(string addCardXml, int userID, out long objectId, out string resultXml, out string errorXml);


        [OperationContract]
        bool UpdateCustomerStatus(string insertXml, out string errorXml);

        [OperationContract]
        string RollBackCustomerDetails(string insertXml, out string errorXml);


        [OperationContract]
        bool ValidateTokenCustomer(string tokenId, out string resultXml);

        // SSC Enhancements

        [OperationContract]
        bool ViewCardRange(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, int rowCount);

        [OperationContract]
        bool UpdateCardRange(string conditionXml, int userID, long objectID, string resultXml);

        [OperationContract]
        bool ViewCardType(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool AddCardRange(string conditionXml, int userID, long objectID, string resultXml);

        [OperationContract]
        bool DeleteCardRange(string conditionXml, int userID, long objectID, string resultXml);

        [OperationContract]
        bool AddCardType(string conditionXml, int userID, long objectID,out string resultXml);

        [OperationContract]
        bool UpdateCardType(string conditionXml, int userID, long objectID, out string resultXml);

        [OperationContract]
        bool ViewStores(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, int rowCount);
        [OperationContract]
        bool ViewStoreRegion(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, int rowCount);
        [OperationContract]
        bool ViewStoreFormat(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, int rowCount);
        [OperationContract]
        bool AddStores(string conditionXml, int userID, long objectID,out string resultXml);
        [OperationContract]
        bool UpdateStores(string conditionXml, int userID, long objectID, out string resultXml);

        [OperationContract]
        bool GetStoreNames(string storeNumbers, out string errorXml, out string resultXml, string culture);

        //To send an XML input to Addprintathomedetail method in CustoerService
        [OperationContract]
        bool AddPrintAtHomeDetailsXMLInput(string updateXml, out string errorXml);
    }
}
