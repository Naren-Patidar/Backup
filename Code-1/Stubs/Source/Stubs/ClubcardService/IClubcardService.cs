using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ClubcardService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(Namespace = "http://tesco.com/clubcardonline/datacontract/2010/01")]
    public interface IClubcardService
    {

        [OperationContract]
        bool AddPrintAtHomeDetailsXMLInput(out string errorXml, string updateXml);

        [OperationContract]
        GetFuelBalanceResponse GetFuelSaverPointsBalance(GetFuelBalanceRequest objReq);

        [OperationContract]
        bool IsXmasClubMember(out string errorXml, out string resultXml, long CustomerID, string culture);

       [OperationContract]
        bool GetChristmasSaverSummary(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool GetMyAccountDetails(out string errorXml, out string resultXml, long CustomerID, string culture);

        [OperationContract]
        bool IsNewOrderReplacementValid(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool AddNewOrderReplacement(out string errorXml, out long customerID, string updateXml, string consumer);

        [OperationContract]
        bool GetHouseholdCustomers(out string errorXml, out string resultXml, long CustomerID, string culture);

        [OperationContract]
        bool GetClubcards(out string errorXml, out string resultXml, long CustomerID, string culture);

        [OperationContract]
        bool GetClubcardsCustomer(out string errorXml, out string resultXml, long CustomerID, string culture);

        [OperationContract]
        bool GetPointsForAllCollPeriodByCustomer(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool GetTxnDetailsByCustomerAndOfferID(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool GetTxnDetailsByHouseholdCustomerAndOfferID(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool GetPointsSummaryInfo(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool CheckCustomerActivated(out char activated, out long customerID, out string errorXml, out string resultXml, long dotcomCustomerID, string culture);

        [OperationContract]
        bool IGHSCheckCustomerActivated(out char activated, out long customerID, out string errorXml, out string resultXml, string dotcomCustomerID, string culture);

        [OperationContract]
        bool CheckHouseholdStatusOfCustomer(out string errorXml, out string resultXml, long customerID, string culture);

        [OperationContract]
        bool AddPrimaryCard(out long objectId, out string resultXml, out string errorXml, string addCardXml, int userID);

        [OperationContract]
        bool UpdateCustomerStatus(out string errorXml, string insertXml);

        [OperationContract]
        string RollBackCustomerDetails(out string errorXml, string insertXml);

        [OperationContract]
        bool ValidateTokenCustomer(out string resultXml, string tokenId);

        [OperationContract]
        bool ViewCardRange(out string errorXml, out string resultXml, string conditionXml, int maxRowCount, string culture, int rowCount);

        [OperationContract]
        bool UpdateCardRange(string conditionXml, int userID, long objectID, string resultXml);

       [OperationContract]
        bool ViewCardType(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool AddCardRange(string conditionXml, int userID, long objectID, string resultXml);

        [OperationContract]
        bool DeleteCardRange(string conditionXml, int userID, long objectID, string resultXml);

        [OperationContract]
        bool AddCardType(out string resultXml, string conditionXml, int userID, long objectID);

       [OperationContract]
        bool UpdateCardType(out string resultXml, string conditionXml, int userID, long objectID);

        [OperationContract]
        bool ViewStores(out string errorXml, out string resultXml, string conditionXml, int maxRowCount, string culture, int rowCount);

        [OperationContract]
        bool ViewStoreRegion(out string errorXml, out string resultXml, string conditionXml, int maxRowCount, string culture, int rowCount);

        [OperationContract]
        bool ViewStoreFormat(out string errorXml, out string resultXml, string conditionXml, int maxRowCount, string culture, int rowCount);

        [OperationContract]
        bool AddStores(out string resultXml, string conditionXml, int userID, long objectID);

        [OperationContract]
        bool UpdateStores(out string resultXml, string conditionXml, int userID, long objectID);

        [OperationContract]
        bool GetStoreNames(out string errorXml, out string resultXml, string storeNumbers, string culture);

        [OperationContract]
        bool ResetOrderReplacementData(out string errorXml, long customerID, string ClubcardNumber, string Culture);
    }


    
}
