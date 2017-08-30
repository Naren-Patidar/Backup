using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Configuration;


namespace ClubcardService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class ClubcardService : IClubcardService
    {
        ClubcardServiceProvider provider = new ClubcardServiceProvider();

        #region IClubcardService Members

        public bool AddPrintAtHomeDetailsXMLInput(out string errorXml, string updateXml)
        {
            throw new NotImplementedException();
        }

        public GetFuelBalanceResponse GetFuelSaverPointsBalance(GetFuelBalanceRequest objReq)
        {
            throw new NotImplementedException();
        }

        public bool IsXmasClubMember(out string errorXml, out string resultXml, long CustomerID, string culture)
        {
            return provider.IsXmasClubMember(out  errorXml, out  resultXml,  CustomerID,  culture);
        }

        public bool GetChristmasSaverSummary(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            return provider.GetChristmasSaverSummary(out  errorXml, out  resultXml, out  rowCount,  conditionXml,  maxRowCount,  culture);
        }

        public bool GetMyAccountDetails(out string errorXml, out string resultXml, long CustomerID, string culture)
        {
            return provider.GetMyAccountDetails(out errorXml, out resultXml, CustomerID, culture);
        }

        public bool IsNewOrderReplacementValid(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            return provider.IsNewOrderReplacementValid(out  errorXml, out  resultXml, out  rowCount,  conditionXml,  maxRowCount,  culture);
        }

        public bool AddNewOrderReplacement(out string errorXml, out long customerID, string updateXml, string consumer)
        {
            return provider.AddNewOrderReplacement(out errorXml, out customerID, updateXml, consumer);
        }

        public bool GetHouseholdCustomers(out string errorXml, out string resultXml, long CustomerID, string culture)
        {
            return provider.GetHouseholdCustomers(out errorXml, out resultXml, CustomerID, culture);
        }

        public bool GetClubcards(out string errorXml, out string resultXml, long CustomerID, string culture)
        {
            throw new NotImplementedException();
        }

        public bool GetClubcardsCustomer(out string errorXml, out string resultXml, long CustomerID, string culture)
        {
            return provider.GetClubcardsCustomer(out errorXml, out resultXml, CustomerID, culture);
        }

        public bool GetPointsForAllCollPeriodByCustomer(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            return provider.GetPointsForAllCollPeriodByCustomer(out  errorXml, out  resultXml, out  rowCount, conditionXml, maxRowCount, culture);
        }

        public bool GetTxnDetailsByCustomerAndOfferID(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            return provider.GetTxnDetailsByCustomerAndOfferID(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture);
        }

        public bool GetTxnDetailsByHouseholdCustomerAndOfferID(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool GetPointsSummaryInfo(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            return provider.GetPointsSummaryInfo(out  errorXml, out  resultXml, out  rowCount,  conditionXml,  maxRowCount,  culture);
        }

        public bool CheckCustomerActivated(out char activated, out long customerID, out string errorXml, out string resultXml, long dotcomCustomerID, string culture)
        {
            throw new NotImplementedException();
        }

        public bool IGHSCheckCustomerActivated(out char activated, out long customerID, out string errorXml, out string resultXml, string dotcomCustomerID, string culture)
        {
            return provider.IGHSCheckCustomerActivated(out activated, out customerID, out errorXml, out resultXml, dotcomCustomerID, culture);
        }

        public bool CheckHouseholdStatusOfCustomer(out string errorXml, out string resultXml, long customerID, string culture)
        {
            throw new NotImplementedException();
        }

        public bool AddPrimaryCard(out long objectId, out string resultXml, out string errorXml, string addCardXml, int userID)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCustomerStatus(out string errorXml, string insertXml)
        {
            throw new NotImplementedException();
        }

        public string RollBackCustomerDetails(out string errorXml, string insertXml)
        {
            throw new NotImplementedException();
        }

        public bool ValidateTokenCustomer(out string resultXml, string tokenId)
        {
            throw new NotImplementedException();
        }

        public bool ViewCardRange(out string errorXml, out string resultXml, string conditionXml, int maxRowCount, string culture, int rowCount)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCardRange(string conditionXml, int userID, long objectID, string resultXml)
        {
            throw new NotImplementedException();
        }

        public bool ViewCardType(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool AddCardRange(string conditionXml, int userID, long objectID, string resultXml)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCardRange(string conditionXml, int userID, long objectID, string resultXml)
        {
            throw new NotImplementedException();
        }

        public bool AddCardType(out string resultXml, string conditionXml, int userID, long objectID)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCardType(out string resultXml, string conditionXml, int userID, long objectID)
        {
            throw new NotImplementedException();
        }

        public bool ViewStores(out string errorXml, out string resultXml, string conditionXml, int maxRowCount, string culture, int rowCount)
        {
            throw new NotImplementedException();
        }

        public bool ViewStoreRegion(out string errorXml, out string resultXml, string conditionXml, int maxRowCount, string culture, int rowCount)
        {
            throw new NotImplementedException();
        }

        public bool ViewStoreFormat(out string errorXml, out string resultXml, string conditionXml, int maxRowCount, string culture, int rowCount)
        {
            throw new NotImplementedException();
        }

        public bool AddStores(out string resultXml, string conditionXml, int userID, long objectID)
        {
            throw new NotImplementedException();
        }

        public bool UpdateStores(out string resultXml, string conditionXml, int userID, long objectID)
        {
            throw new NotImplementedException();
        }

        public bool GetStoreNames(out string errorXml, out string resultXml, string storeNumbers, string culture)
        {
            throw new NotImplementedException();
        }

        public bool ResetOrderReplacementData(out string errorXml, long customerID, string ClubcardNumber, string Culture)
        {
            return provider.ResetOrderReplacementData(out  errorXml, customerID, ClubcardNumber,Culture);
        }


        #endregion
    }
}
