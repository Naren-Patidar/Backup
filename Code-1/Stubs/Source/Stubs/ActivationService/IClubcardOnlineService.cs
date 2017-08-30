using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CustomerActivationServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IClubcardOnlineService
    {
        [OperationContract]
        ClubcardOnline.Web.Entities.CustomerActivationServices.ValidateClubcardAccountExistsResponse ValidateClubcardAccountExists(long ClubcardNumber, string PostCode);

        [OperationContract]
        ClubcardOnline.Web.Entities.CustomerActivationServices.AccountFindByClubcardNumberResponse AccountFindByClubcardNumber(long ClubcardNumber, ClubcardOnline.Web.Entities.CustomerActivationServices.ClubcardCustomer customer, System.Data.DataSet dsConfig);
       
        [OperationContract]
        ClubcardOnline.Web.Entities.CustomerActivationServices.AccountFindByClubcardNumberResponse AccountFindByClubcardNumberXmlParameter(long ClubcardNumber, ClubcardOnline.Web.Entities.CustomerActivationServices.ClubcardCustomer customer, string ConfigXml);

        [OperationContract]
        ClubcardOnline.Web.Entities.CustomerActivationServices.AccountDuplicateCheckResponse AccountDuplicateCheck(ClubcardOnline.Web.Entities.CustomerActivationServices.ClubcardCustomer customer);

        [OperationContract]
        ClubcardOnline.Web.Entities.CustomerActivationServices.AccountCreateResponse AccountCreate(long dotcomCustomerID, ClubcardOnline.Web.Entities.CustomerActivationServices.ClubcardCustomer customer, string source, string isDuplicate);

        [OperationContract]
        ClubcardOnline.Web.Entities.CustomerActivationServices.AccountLinkResponse AccountLink(long dotcomCustomerID, long clubCardNumber);

        [OperationContract]
        ClubcardOnline.Web.Entities.CustomerActivationServices.AccountLinkResponse IGHSAccountLink(string dotcomCustomerID, long clubCardNumber);

        [OperationContract]
        ClubcardOnline.Web.Entities.CustomerActivationServices.AccountUpdateResponse AccountUpdate(long dotcomCustomerID, long clubCardNumber, ClubcardOnline.Web.Entities.CustomerActivationServices.ClubcardCustomer customer);

        [OperationContract]
        bool SendActivationEmail(string strTo);

        [OperationContract]
        bool SendJoinConfirmationEmail(string strTo, string title, string custName, long clubcardID);

        // TODO: Add your service operations here
    }

}
