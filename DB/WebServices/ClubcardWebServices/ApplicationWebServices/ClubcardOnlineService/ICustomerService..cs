using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Security;
using System.Data;
using Tesco.com.ClubcardOnlineService;
using ClubcardOnlineService.ClubcardCouponService;



namespace Tesco.com.ClubcardOnlineService
{
    // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in App.config.
    [ServiceContract(Namespace = "http://tesco.com/clubcardonline/datacontract/2010/01")]
    public interface ICustomerService
    {
        
        [OperationContract]
        bool GetCustomerDetails(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool UpdateCustomerDetails(string updateXml, string consumer, out string errorXml, out Int64 customerID);

        [OperationContract]
        bool GetCustomerPreferences(Int64 CustomerID, string culture, out string errorXml, out string resultXml);

        [OperationContract]
        bool UpdateCustomerPreferences(string updateXml, string consumer, out string errorXml, out Int64 customerID, char level);

        [OperationContract]
        bool SearchCustomer(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool GetTitles(out string errorXml, out string resultXml, out int rowCount, string culture);

        [OperationContract]
        bool AuthenticateUser(string domain, string userName, string password, string cultureCode, string AppName, string LDPath, out string errorXml, out string resultXml, out int userID);

        // MCA new functions

        [OperationContract]
        bool GetConfigDetails(string conditionXml, out string errorXml, out string resultXml, out int rowCount, string culture);

        [OperationContract]
        bool UpdateConfig(string updateXml, string consumer, out string errorXml, out Int64 customerID);

        [OperationContract]
        bool AddPrintAtHomeDetails(DataSet updateDS, out string errorXml);


        [OperationContract]
        string InsertCustomerDetails(string insertXml, out string errorXml);

        [OperationContract]
        bool GetSecretQtns(out string errorXml, out string resultXml, out int rowCount, string culture);

        //[OperationContract]
        //bool GetExistingCustomerDetails(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);
        [OperationContract]
        bool GetExistingCustomerDetails(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);
        [OperationContract]
        bool UpdateExistingCustomerDetails(string objectXml, out string resultXml);
        [OperationContract]
        bool GetPrimaryCard(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);
        [OperationContract]
        bool AddPhoneNoToAccount(string objectXml, out string resultXml);
        [OperationContract]
        bool EmailValidation(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);
        [OperationContract]
        bool PhoneNoValidation(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);
        [OperationContract]
        bool AlternateIdValidation(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);
        [OperationContract]
        bool GetCustIdByEmailId(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);
        [OperationContract]
        bool InsertCardNo(string insertXml, out string errorXml);
        [OperationContract]
        bool GetCardNoByCustId(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);
        [OperationContract]
        bool GetTransactionHistory(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);
        [OperationContract]
        bool GetFriendCustomerDetails(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);
        [OperationContract]
        bool CreateUser(string objectXml, out string UserStatus);
        /*Menu*/
        [OperationContract]
        bool GetAccountOverviewDetails(Int64 customerID, out string errorXml, out string resultXml);
        /*Menu*/

        /* My Profile -Sathesh*/
        //[OperationContract]
        //bool UpdateExistingCustomerProfileDetails(string updateXml, out string errorXml);

        //[OperationContract]
        //bool PhoneNoValidation(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool GetAboutmeDetails(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool UpdateAboutMeDetails(string updateXml, out string errorXml);

        [OperationContract]
        bool UpdateDietaryPreferences(string updateXml, out string errorXml);

        [OperationContract]
        bool UpdateMyProfileDetails(string updateXml, out string errorXml);

        [OperationContract]
        bool GetMyProfileDetails(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool AddSupplementaryCard(string objectXml, out long objectId, out string errorXml, out string resultXml);

        [OperationContract]
        bool Changepassword(Int64 custtomerID, string oldPassword, string newPassword);
        [OperationContract]
        string GetSecretQuestion(Int64 custtomerID);
        [OperationContract]
        bool UpdateEmailPreferences(string updateXml, out string errorXml);

        [OperationContract]
        bool GetEmailPreferences(Int64 customerID, out string errorXml, out string resultXml);

        [OperationContract]
        bool UpdateSecretQns(Int64 customerID, string question, string answer);

        [OperationContract]
        bool UpdateEmailAddresss(string newUserName, Int64 CustomerId);


        /* END of My Profile -Sathesh*/

        /* My Profile -Sudhakar*/

        // added as part ofUS loyalty Program
        [OperationContract]
        bool GetCustomerRewards(Int64 PrimaryCardNumber, out float Rewards);
        [OperationContract]
        bool ConvertPointsToCash(Int64 PrimaryCardID, Int32 TotalPoints);

        [OperationContract]
        bool GetPointsExpiryDetails(Int64 PrimaryCardID, out string errorXml, out string resultXml);

        [OperationContract]
        bool GetRewardExpiryDate(Int64 PrimaryCardID, out string errorXml, out string resultXml);

        /* END of My Profile -Sudhakar*/
        //ReActivate Account

        [OperationContract]
        bool GetCustUseMailStatus(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool UpdateUseMailStatus(string updateXml, out string errorXml);

        //Reward Card Management -- Kavitha
        [OperationContract]
        bool UpdateCardStatus(string insertXml, out string errorXml, out string errorMessage);

        [OperationContract]
        bool GetAllClubcards(Int64 customerID, string culture, out string errorXml, out string resultXml);

        [OperationContract]
        bool UpdatePhoneNumber(string insertXml, out string errorXml);

        [OperationContract]
        bool UpdatePrimaryClubcardID(string insertXml, out string errorXml);
        //End of Reward Card Management

        #region Sign In

        [OperationContract]
        bool UpdateCustomerTermsNConstionsStatus(Int64 customerID, out string errorXml);

        [OperationContract]
        bool GetCustomerStatus(string username, out string errorXml, out string resultXml);

        [OperationContract]
        bool GetCustomerIDSecurityDB(string userName, out Int64 customerID);

        [OperationContract]
        bool ValidateUser(string username, string password);

        [OperationContract]
        MembershipUser GetUser(string userName);

        [OperationContract]
        void UpdateUser(MembershipUser muser);

        [OperationContract]
        bool Resetpwd(MembershipUser muser, string secretans);

        [OperationContract]
        bool GetCustomerTNCInfo(string userName, out string errorXml, out string resultXml);

        [OperationContract]
        bool ValidateToken(string tokenId, out string resultXml);

        [OperationContract]
        bool CreateToken(string userName, out string resultXml);

        [OperationContract]
        bool ExpireToken(string tokenId);

        [OperationContract]
        bool SendEmailET(string email, string linkUrl, string emailType);
        /* END of My Profile -Sudhakar*/

        #endregion

        #region My Coupons -- Noushad

        [OperationContract]
        bool GetCoupons(Int64 customerID, out string resultXml);

        [OperationContract]
        bool UpdateCoupons(Int64 customerID, string couponStatus);

        #endregion

        #region CSC -Neeta
        [OperationContract]
        bool GetGroupDetails(out string errorXml, out string resultXml, string insertXml, string culture);

        [OperationContract]
        bool Add(string objectXml, int sessionUserID, out long objectId, out string resultXml, out string errorXml);

        [OperationContract]
        bool SearchUser(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool Update(string objectXml, int sessionUserID, out long objectId, out string resultXml, out string errorXml);

        [OperationContract]
        string GetCustomerID(Int64 customerID);

        [OperationContract]
        bool AddCustomer(string objectXml, int sessionUserID, out string resultXml, out string errorXml);

        [OperationContract]
        bool GetCardStatus(out string errorXml, out string resultXml);

        [OperationContract]
        bool TransactionsByOffer(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool GetTransactionReasonCode(out string errorXml, out string resultXml, string culture);

        [OperationContract]
        bool GetPointsBalance(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool AddPoints(string objectXml, out long objectId, out string errorXml, out string resultXml, int userID);

        [OperationContract]
        bool GetPointsSummary(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool Test(string insertXml, out string errorXml);
        #endregion

        #region DelinkingAccounts
        [OperationContract]
        bool GetAlternativeIds(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool DeLinkingDotcomAccounts(string objectXml, out string resultXml);

        #endregion


        [OperationContract]
        bool LoadPreferences(out string errorXml, out string resultXml, out int rowCount, string culture);

        [OperationContract]
        string ValidateEmailLink(string guid);
        [OperationContract]
        CouponResponse GetCustomerCoupons(CouponRequest objCouponRequest);

        [OperationContract]
        CouponResponse IGHSGetCustomerCoupons(CustomerCouponRequest objCouponRequest);

        #region SecurityLayer - Madhu

        [OperationContract]
        bool InsertUpdateCustomerVerificationDetails(string updateXml,out Int64 customerID,  out string resultXml );

        [OperationContract]
        bool GetCustomerVerificationDetails(string conditionXml, out string errorXml, out string resultXml);

        #endregion

    }



    [DataContract]
    public enum ConsumerSource
    {
        [EnumMember]
        Mobile,
        [EnumMember]
        Dotcom,
        [EnumMember]
        Kiosk,
        [EnumMember]
        MCA,
        [EnumMember]
        Other
    };


    //Mobile API data contracts
    //begin
    
    [DataContract]
    public class CouponRequest
    {
        
        private Int64 _dotcomId;

        //Declare an enum with multiple user types as its elements

        [DataMember(IsRequired=true)]
        public Int64 DotComID
        {
            get { return _dotcomId; }
            set { _dotcomId = value; }
        }

        [DataMember]
        public ConsumerSource Source
        {
            get;
            set;

        }

    }
	
	[DataContract]
    public class CustomerCouponRequest
    {

        private string _dotcomId;

        //Declare an enum with multiple user types as its elements

        [DataMember(IsRequired = true)]
        public string DotComID
        {
            get { return _dotcomId; }
            set { _dotcomId = value; }
        }

        [DataMember]
        public ConsumerSource Source
        {
            get;
            set;

        }

    }
 
    [DataContract]
    public class CouponResponse
    {
        
        private bool _status = true;
        private string _errorMessage;
        private List<CouponInformation> _coupons;
        private int _totalCoupon;
        private int _activeCoupon;
        private long _houseHoldId;
        [DataMember]
        public bool Status
        {

            get { return _status; }
            set { _status = value; }
        }

        [DataMember]
        public string ErrorMessage
        {

            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        [DataMember]
        public List<CouponInformation> Coupons
        {

            get { return _coupons; }
            set { _coupons = value; }
        }
        [DataMember]
        public int TotalCoupon
        {

            get { return _totalCoupon; }
            set { _totalCoupon = value; }
        }
        [DataMember]
        public int ActiveCoupon
        {

            get { return _activeCoupon; }
            set { _activeCoupon = value; }
        }
        [DataMember]
        public long HouseHolId
        {

            get { return _houseHoldId; }
            set { _houseHoldId = value; }
        }
    }

    //end

    

   

}
