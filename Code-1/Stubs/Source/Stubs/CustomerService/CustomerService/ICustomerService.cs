using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CustomerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(Namespace = "http://tesco.com/clubcardonline/datacontract/2010/01")]
    public interface ICustomerService
    {
        [OperationContract]
        bool SearchUser(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool Update(out long objectId, out string resultXml, out string errorXml, string objectXml, int sessionUserID);

        [OperationContract]
        string GetCustomerID(long customerID);

        [OperationContract]
        bool AddCustomer(out string resultXml, out string errorXml, string objectXml, int sessionUserID);

        [OperationContract]
        bool GetCardStatus(out string errorXml, out string resultXml);

        [OperationContract]
        bool TransactionsByOffer(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool GetTransactionReasonCode(out string errorXml, out string resultXml, string culture);

        [OperationContract]
        bool GetPointsBalance(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

       [OperationContract]
        bool AddPoints(out long objectId, out string errorXml, out string resultXml, string objectXml, int userID);

        [OperationContract]
        bool GetPointsSummary(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool Test(out string errorXml, string insertXml);

        [OperationContract]
        bool GetAlternativeIds(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool DeLinkingDotcomAccounts(out string resultXml, string objectXml);

        [OperationContract]
        bool LoadPreferences(out string errorXml, out string resultXml, out int rowCount, string culture);

        [OperationContract]
        string ValidateEmailLink(string guid);

        [OperationContract]
        CouponResponse GetCustomerCoupons(CouponRequest objCouponRequest);

        [OperationContract]
        CouponResponse IGHSGetCustomerCoupons(CustomerCouponRequest objCouponRequest);

        [OperationContract]
        bool InsertUpdateCustomerVerificationDetails(out long customerID, out string resultXml, string updateXml);

        [OperationContract]
        bool GetCustomerVerificationDetails(out string errorXml, out string resultXml, string conditionXml);

        [OperationContract]
        bool Changepassword(long custtomerID, string oldPassword, string newPassword);

        [OperationContract]
        string GetSecretQuestion(long custtomerID);

        [OperationContract]
        bool UpdateEmailPreferences(out string errorXml, string updateXml);

        [OperationContract]
        bool GetEmailPreferences(out string errorXml, out string resultXml, long customerID);

        [OperationContract]
        bool UpdateSecretQns(long customerID, string question, string answer);

        [OperationContract]
        bool UpdateEmailAddresss(string newUserName, long CustomerId);

        [OperationContract]
        bool GetCustomerRewards(out float Rewards, long PrimaryCardNumber);

        [OperationContract]
        bool ConvertPointsToCash(long PrimaryCardID, int TotalPoints);

        [OperationContract]
        bool GetPointsExpiryDetails(out string errorXml, out string resultXml, long PrimaryCardID);

        [OperationContract]
        bool GetRewardExpiryDate(out string errorXml, out string resultXml, long PrimaryCardID);

        [OperationContract]
        bool GetCustUseMailStatus(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool UpdateUseMailStatus(out string errorXml, string updateXml);

        [OperationContract]
        bool UpdateCardStatus(out string errorXml, out string errorMessage, string insertXml);

        [OperationContract]
        bool GetAllClubcards(out string errorXml, out string resultXml, long customerID, string culture);

        [OperationContract]
        bool UpdatePhoneNumber(out string errorXml, string insertXml);

        [OperationContract]
        bool UpdatePrimaryClubcardID(out string errorXml, string insertXml);

        [OperationContract]
        bool UpdateCustomerTermsNConstionsStatus(out string errorXml, long customerID);

        [OperationContract]
        bool GetCustomerStatus(out string errorXml, out string resultXml, string username);

        [OperationContract]
        bool GetCustomerIDSecurityDB(out long customerID, string userName);

        [OperationContract]
        bool ValidateUser(string username, string password);

        [OperationContract]
        MembershipUser GetUser(string userName);

        [OperationContract]
        void UpdateUser(MembershipUser muser);

        [OperationContract]
        bool Resetpwd(MembershipUser muser, string secretans);

        [OperationContract]
        bool GetCustomerTNCInfo(out string errorXml, out string resultXml, string userName);

        [OperationContract]
        bool ValidateToken(out string resultXml, string tokenId);

        [OperationContract]
        bool CreateToken(out string resultXml, string userName);

        [OperationContract]
        bool ExpireToken(string tokenId);

        [OperationContract]
        bool SendEmailET(string email, string linkUrl, string emailType);

        [OperationContract]
        bool GetCoupons(out string resultXml, long customerID);

        [OperationContract]
        bool UpdateCoupons(long customerID, string couponStatus);

        [OperationContract]
        bool GetGroupDetails(out string errorXml, out string resultXml, string insertXml, string culture);

        [OperationContract]
        bool Add(out long objectId, out string resultXml, out string errorXml, string objectXml, int sessionUserID);

        [OperationContract]
        bool GetCustomerDetails(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool UpdateCustomerDetails(out string errorXml, out long customerID, string updateXml, string consumer);

        [OperationContract]
        bool GetCustomerPreferences(out string errorXml, out string resultXml, long CustomerID, string culture);

        [OperationContract]
        bool UpdateCustomerPreferences(out string errorXml, out long customerID, string updateXml, string consumer, char level);

        [OperationContract]
        bool SearchCustomer(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool GetTitles(out string errorXml, out string resultXml, out int rowCount, string culture);

        [OperationContract]
        bool AuthenticateUser(out string errorXml, out string resultXml, out int userID, string domain, string userName, string password, string cultureCode, string AppName, string LDPath);

        [OperationContract]
        bool GetConfigDetails(out string errorXml, out string resultXml, out int rowCount, string conditionXml, string culture);

        [OperationContract]
        bool UpdateConfig(out string errorXml, out long customerID, string updateXml, string consumer);

        [OperationContract]
        bool AddPrintAtHomeDetails(out string errorXml, System.Data.DataSet updateDS);

        [OperationContract]
        string InsertCustomerDetails(out string errorXml, string insertXml);

        [OperationContract]
        bool GetSecretQtns(out string errorXml, out string resultXml, out int rowCount, string culture);

        [OperationContract]
        bool GetExistingCustomerDetails(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool UpdateExistingCustomerDetails(out string resultXml, string objectXml);

        [OperationContract]
        bool GetPrimaryCard(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool AddPhoneNoToAccount(out string resultXml, string objectXml);

        [OperationContract]
        bool EmailValidation(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool PhoneNoValidation(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool AlternateIdValidation(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool GetCustIdByEmailId(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool InsertCardNo(out string errorXml, string insertXml);

        [OperationContract]
        bool GetCardNoByCustId(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool GetTransactionHistory(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool GetFriendCustomerDetails(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool CreateUser(out string UserStatus, string objectXml);

        [OperationContract]
        bool GetAccountOverviewDetails(out string errorXml, out string resultXml, long customerID);

        [OperationContract]
        bool GetAboutmeDetails(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool UpdateAboutMeDetails(out string errorXml, string updateXml);

        [OperationContract]
        bool UpdateDietaryPreferences(out string errorXml, string updateXml);

        [OperationContract]
        bool UpdateMyProfileDetails(out string errorXml, string updateXml);

        [OperationContract]
        bool GetMyProfileDetails(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture);

        [OperationContract]
        bool AddSupplementaryCard(out long objectId, out string errorXml, out string resultXml, string objectXml);
    }


}
