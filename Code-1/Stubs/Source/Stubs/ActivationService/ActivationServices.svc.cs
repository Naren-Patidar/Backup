using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.IO;
using ServiceUtility;
using ClubcardOnline.Web.Entities.CustomerActivationServices;
using System.Configuration;

namespace CustomerActivationServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
     [GlobalErrorHandlerBehaviourAttribute(typeof(GlobalErrorHandler))]
    public class ActivationServices : IClubcardOnlineService
    {
        ActivationServiceProvider provider = new ActivationServiceProvider();
      
        public ClubcardOnline.Web.Entities.CustomerActivationServices.ValidateClubcardAccountExistsResponse ValidateClubcardAccountExists(long ClubcardNumber, string PostCode)
        {
            throw new NotImplementedException();
        }

        public ClubcardOnline.Web.Entities.CustomerActivationServices.AccountFindByClubcardNumberResponse AccountFindByClubcardNumber(long ClubcardNumber, ClubcardOnline.Web.Entities.CustomerActivationServices.ClubcardCustomer customer, System.Data.DataSet dsConfig)
        {
            return provider.AccountFindByClubcardNumber(ClubcardNumber,customer,dsConfig);
        }

        public ClubcardOnline.Web.Entities.CustomerActivationServices.AccountFindByClubcardNumberResponse AccountFindByClubcardNumberXmlParameter(long ClubcardNumber, ClubcardOnline.Web.Entities.CustomerActivationServices.ClubcardCustomer customer, string ConfigXml)
        {
            throw new NotImplementedException();
        }

        public ClubcardOnline.Web.Entities.CustomerActivationServices.AccountDuplicateCheckResponse AccountDuplicateCheck(ClubcardOnline.Web.Entities.CustomerActivationServices.ClubcardCustomer customer)
        {
            throw new NotImplementedException();
        }

        public ClubcardOnline.Web.Entities.CustomerActivationServices.AccountCreateResponse AccountCreate(long dotcomCustomerID, ClubcardOnline.Web.Entities.CustomerActivationServices.ClubcardCustomer customer, string source, string isDuplicate)
        {
            throw new NotImplementedException();
        }

        public ClubcardOnline.Web.Entities.CustomerActivationServices.AccountLinkResponse AccountLink(long dotcomCustomerID, long clubCardNumber)
        {
            throw new NotImplementedException();
        }

        public ClubcardOnline.Web.Entities.CustomerActivationServices.AccountLinkResponse IGHSAccountLink(string dotcomCustomerID, long clubCardNumber)
        {
            return provider.IGHSAccountLink(dotcomCustomerID, clubCardNumber);
        }

        public ClubcardOnline.Web.Entities.CustomerActivationServices.AccountUpdateResponse AccountUpdate(long dotcomCustomerID, long clubCardNumber, ClubcardOnline.Web.Entities.CustomerActivationServices.ClubcardCustomer customer)
        {
            throw new NotImplementedException();
        }

        public bool SendActivationEmail(string strTo)
        {
            return provider.SendActivationEmail(strTo);
        }

        public bool SendJoinConfirmationEmail(string strTo, string title, string custName, long clubcardID)
        {
            throw new NotImplementedException();
        }
    }
}
