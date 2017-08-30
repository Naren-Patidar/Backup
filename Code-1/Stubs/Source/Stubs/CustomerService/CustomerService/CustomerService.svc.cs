using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using CustomerService.Provider;
using System.Xml;
using System.Configuration;


namespace CustomerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class CustomerService : ICustomerService
    {
        CustomerServiceProvider provider = new CustomerServiceProvider();
        #region ICustomerService Members

        public bool SearchUser(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool Update(out long objectId, out string resultXml, out string errorXml, string objectXml, int sessionUserID)
        {
            throw new NotImplementedException();
        }

        public string GetCustomerID(long customerID)
        {
            throw new NotImplementedException();
        }

        public bool AddCustomer(out string resultXml, out string errorXml, string objectXml, int sessionUserID)
        {
            throw new NotImplementedException();
        }

        public bool GetCardStatus(out string errorXml, out string resultXml)
        {
            throw new NotImplementedException();
        }

        public bool TransactionsByOffer(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool GetTransactionReasonCode(out string errorXml, out string resultXml, string culture)
        {
            throw new NotImplementedException();
        }

        public bool GetPointsBalance(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool AddPoints(out long objectId, out string errorXml, out string resultXml, string objectXml, int userID)
        {
            throw new NotImplementedException();
        }

        public bool GetPointsSummary(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool Test(out string errorXml, string insertXml)
        {
            throw new NotImplementedException();
        }

        public bool GetAlternativeIds(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool DeLinkingDotcomAccounts(out string resultXml, string objectXml)
        {
            throw new NotImplementedException();
        }

        public bool LoadPreferences(out string errorXml, out string resultXml, out int rowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public string ValidateEmailLink(string guid)
        {
            throw new NotImplementedException();
        }

        public CouponResponse GetCustomerCoupons(CouponRequest objCouponRequest)
        {
            throw new NotImplementedException();
        }

        public CouponResponse IGHSGetCustomerCoupons(CustomerCouponRequest objCouponRequest)
        {
            throw new NotImplementedException();
        }

        public bool InsertUpdateCustomerVerificationDetails(out long customerID, out string resultXml, string updateXml)
        {
            return provider.InsertUpdateCustomerVerificationDetails(out customerID, out resultXml, updateXml);
        }

        public bool GetCustomerVerificationDetails(out string errorXml, out string resultXml, string conditionXml)
        {
            return provider.GetCustomerVerificationDetails(out errorXml, out resultXml, conditionXml);
        }

        public bool Changepassword(long custtomerID, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public string GetSecretQuestion(long custtomerID)
        {
            throw new NotImplementedException();
        }

        public bool UpdateEmailPreferences(out string errorXml, string updateXml)
        {
            throw new NotImplementedException();
        }

        public bool GetEmailPreferences(out string errorXml, out string resultXml, long customerID)
        {
            throw new NotImplementedException();
        }

        public bool UpdateSecretQns(long customerID, string question, string answer)
        {
            throw new NotImplementedException();
        }

        public bool UpdateEmailAddresss(string newUserName, long CustomerId)
        {
            throw new NotImplementedException();
        }

        public bool GetCustomerRewards(out float Rewards, long PrimaryCardNumber)
        {
            throw new NotImplementedException();
        }

        public bool ConvertPointsToCash(long PrimaryCardID, int TotalPoints)
        {
            throw new NotImplementedException();
        }

        public bool GetPointsExpiryDetails(out string errorXml, out string resultXml, long PrimaryCardID)
        {
            throw new NotImplementedException();
        }

        public bool GetRewardExpiryDate(out string errorXml, out string resultXml, long PrimaryCardID)
        {
            throw new NotImplementedException();
        }

        public bool GetCustUseMailStatus(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUseMailStatus(out string errorXml, string updateXml)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCardStatus(out string errorXml, out string errorMessage, string insertXml)
        {
            throw new NotImplementedException();
        }

        public bool GetAllClubcards(out string errorXml, out string resultXml, long customerID, string culture)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePhoneNumber(out string errorXml, string insertXml)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePrimaryClubcardID(out string errorXml, string insertXml)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCustomerTermsNConstionsStatus(out string errorXml, long customerID)
        {
            throw new NotImplementedException();
        }

        public bool GetCustomerStatus(out string errorXml, out string resultXml, string username)
        {
            throw new NotImplementedException();
        }

        public bool GetCustomerIDSecurityDB(out long customerID, string userName)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public MembershipUser GetUser(string userName)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(MembershipUser muser)
        {
            throw new NotImplementedException();
        }

        public bool Resetpwd(MembershipUser muser, string secretans)
        {
            throw new NotImplementedException();
        }

        public bool GetCustomerTNCInfo(out string errorXml, out string resultXml, string userName)
        {
            throw new NotImplementedException();
        }

        public bool ValidateToken(out string resultXml, string tokenId)
        {
            throw new NotImplementedException();
        }

        public bool CreateToken(out string resultXml, string userName)
        {
            throw new NotImplementedException();
        }

        public bool ExpireToken(string tokenId)
        {
            throw new NotImplementedException();
        }

        public bool SendEmailET(string email, string linkUrl, string emailType)
        {
            throw new NotImplementedException();
        }

        public bool GetCoupons(out string resultXml, long customerID)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCoupons(long customerID, string couponStatus)
        {
            throw new NotImplementedException();
        }

        public bool GetGroupDetails(out string errorXml, out string resultXml, string insertXml, string culture)
        {
            throw new NotImplementedException();
        }

        public bool Add(out long objectId, out string resultXml, out string errorXml, string objectXml, int sessionUserID)
        {
            throw new NotImplementedException();
        }

        public bool GetCustomerDetails(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            return provider.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture);
        }

        public bool UpdateCustomerDetails(out string errorXml, out long customerID, string updateXml, string consumer)
        {
            string culture = ConfigurationManager.AppSettings.AllKeys.Contains("culture") ? ConfigurationManager.AppSettings["culture"] : "en-GB";
            return provider.UpdateCustomerDetails(out errorXml, out customerID, updateXml, consumer, culture);
        }

        public bool GetCustomerPreferences(out string errorXml, out string resultXml, long CustomerID, string culture)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCustomerPreferences(out string errorXml, out long customerID, string updateXml, string consumer, char level)
        {
            throw new NotImplementedException();
        }

        public bool SearchCustomer(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            return provider.SearchCustomer(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture);
        }

        public bool GetTitles(out string errorXml, out string resultXml, out int rowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(out string errorXml, out string resultXml, out int userID, string domain, string userName, string password, string cultureCode, string AppName, string LDPath)
        {
            throw new NotImplementedException();
        }

        public bool GetConfigDetails(out string errorXml, out string resultXml, out int rowCount, string conditionXml, string culture)
        {
            return provider.GetConfigDetails(out  errorXml, out resultXml, out rowCount, conditionXml, culture);
        }

        public bool UpdateConfig(out string errorXml, out long customerID, string updateXml, string consumer)
        {
            throw new NotImplementedException();
        }

        public bool AddPrintAtHomeDetails(out string errorXml, System.Data.DataSet updateDS)
        {
            throw new NotImplementedException();
        }

        public string InsertCustomerDetails(out string errorXml, string insertXml)
        {
            throw new NotImplementedException();
        }

        public bool GetSecretQtns(out string errorXml, out string resultXml, out int rowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool GetExistingCustomerDetails(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool UpdateExistingCustomerDetails(out string resultXml, string objectXml)
        {
            throw new NotImplementedException();
        }

        public bool GetPrimaryCard(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool AddPhoneNoToAccount(out string resultXml, string objectXml)
        {
            throw new NotImplementedException();
        }

        public bool EmailValidation(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool PhoneNoValidation(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool AlternateIdValidation(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool GetCustIdByEmailId(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool InsertCardNo(out string errorXml, string insertXml)
        {
            throw new NotImplementedException();
        }

        public bool GetCardNoByCustId(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool GetTransactionHistory(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool GetFriendCustomerDetails(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool CreateUser(out string UserStatus, string objectXml)
        {
            throw new NotImplementedException();
        }

        public bool GetAccountOverviewDetails(out string errorXml, out string resultXml, long customerID)
        {
            throw new NotImplementedException();
        }

        public bool GetAboutmeDetails(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAboutMeDetails(out string errorXml, string updateXml)
        {
            throw new NotImplementedException();
        }

        public bool UpdateDietaryPreferences(out string errorXml, string updateXml)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMyProfileDetails(out string errorXml, string updateXml)
        {
            throw new NotImplementedException();
        }

        public bool GetMyProfileDetails(out string errorXml, out string resultXml, out int rowCount, string conditionXml, int maxRowCount, string culture)
        {
            throw new NotImplementedException();
        }

        public bool AddSupplementaryCard(out long objectId, out string errorXml, out string resultXml, string objectXml)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
