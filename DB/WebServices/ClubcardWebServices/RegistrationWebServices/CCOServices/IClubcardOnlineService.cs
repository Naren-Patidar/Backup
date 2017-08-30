using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using Tesco.com.ClubcardOnline.Entities;
using System.Data;

namespace Tesco.com.ClubcardOnline.WebService
{
    // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in App.config.
    [ServiceContract]
    public interface IClubcardOnlineService
    {
        [OperationContract]
        Messages.ValidateClubcardAccountExistsResponse ValidateClubcardAccountExists(long ClubcardNumber, string PostCode);

        [OperationContract]
        Messages.AccountFindByClubcardNumberResponse AccountFindByClubcardNumber(long ClubcardNumber, ClubcardCustomer customer, DataSet dsConfig);

        //Added to send XML input
        [OperationContract]
        Messages.AccountFindByClubcardNumberResponse AccountFindByClubcardNumberXmlParameter(long ClubcardNumber, ClubcardCustomer customer, string ConfigXml);

        [OperationContract]
        Messages.AccountDuplicateCheckResponse AccountDuplicateCheck(ClubcardCustomer customer);

        [OperationContract]
        Messages.AccountCreateResponse AccountCreate(long dotcomCustomerID, ClubcardCustomer customer, string source, string isDuplicate);

        [OperationContract]
        Messages.AccountLinkResponse AccountLink(long dotcomCustomerID, long clubCardNumber);

        [OperationContract]
        Messages.AccountLinkResponse IGHSAccountLink(string dotcomCustomerID, long clubCardNumber);

        [OperationContract]
        Messages.AccountUpdateResponse AccountUpdate(long dotcomCustomerID, long clubCardNumber, ClubcardCustomer customer);

        [OperationContract]
        bool SendActivationEmail(string strTo);

        [OperationContract]
        bool SendJoinConfirmationEmail(string strTo, string title, string custName, long clubcardID);
    }
}
