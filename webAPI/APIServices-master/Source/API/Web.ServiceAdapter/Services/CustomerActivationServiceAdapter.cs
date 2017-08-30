using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;
using Tesco.ClubcardProducts.MCA.API.Common;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.API.Common.Entities;
using Activation = Tesco.ClubcardProducts.MCA.API.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerActivationServices;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.API.Contracts;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Tesco.ClubcardProducts.MCA.API.ServiceAdapter.Services
{
    public class CustomerActivationServiceAdapter : BaseNGCAdapter, IServiceAdapter
    {
        private IClubcardOnlineService _activationServiceClient;
        DateTime _dtStart = DateTime.UtcNow; 

        #region Constructors

        public CustomerActivationServiceAdapter()
        {
        }

        public CustomerActivationServiceAdapter(string dotcomid, string uuid, string culture)
            : base(dotcomid, uuid, culture)
        {
            this._activationServiceClient = new ClubcardOnlineServiceClient();
        }

        #endregion Constructors

        #region IServiceAdapter Members
        
        public Dictionary<string, object> GetSupportedOperations()
        {
            return new Dictionary<string, object>() 
            { 
                { "GetClubcardAccountDetails", new Activation.AccountFindByClubcardResponse() },
                { "RegisterDotcomIdToCustomerAccount", null },
                { "SendActivationEmail", bool.FalseString }
            };

        }
        
        public string GetName()
        {
            return "activationservice";
        }
        
        public APIResponse Execute(APIRequest request)
        {
            APIResponse response = new APIResponse();
            try
            {
                switch (request.operation.ToLower())
                {
                    case "getclubcardaccountdetails":
                        response.data = this.GetClubcardAccountDetails(
                                                request.GetParameter<string>("clubcardNumber"),
                                                request.GetParameter<string>("customerEntityText"),
                                                request.GetParameter<string>("dsConfigurationText"));
                        break;

                    case "registerdotcomidtocustomeraccount":
                        response.data = this.RegisterDotcomIdToCustomerAccount(
                                                request.GetParameter<String>("dotcomcustomerid"),
                                                request.GetParameter<string>("clubcardnumber"));
                        break;

                    case "sendactivationemail":
                        response.data = this.SendActivationEmail(
                                                request.GetParameter<string>("toemailid"));
                        break;
                }
            }
            catch (Exception ex)
            {
                response.errors.Add(new KeyValuePair<string, string>("ERR-ACTIVATION-SERVICE", ex.ToString()));
            }
            finally
            {
                response.servicestats = this._internalStats.ToString();
            }
            return response;
        }

        #endregion

        #region Private Members

        private Activation.AccountFindByClubcardResponse GetClubcardAccountDetails(
            string clubcardNumber, string customerEntityText, string dsConfigurationText)
        {
            try
            {
                long lClubcardNumber = clubcardNumber.TryParse<long>();
                if (lClubcardNumber == default(long))
                {
                    throw new Exception("Parameter clubcardNumber is mandatory and must be passed for further processing.");
                }

                AccountFindByClubcardNumberResponse accountFindResponse = new AccountFindByClubcardNumberResponse();
                Activation.ClubcardCustomer customerEntity = JsonConvert.DeserializeObject<Activation.ClubcardCustomer>(customerEntityText);
                string xmlEntity = customerEntity.ToXmlString();
                ClubcardCustomer custEntity = xmlEntity.ToObject<ClubcardCustomer>();
                DataSet dsConfiguration = JsonConvert.DeserializeObject<DataSet>(dsConfigurationText);

                this._dtStart = DateTime.UtcNow;
                accountFindResponse = _activationServiceClient.AccountFindByClubcardNumber(lClubcardNumber, custEntity, dsConfiguration);

                Activation.AccountFindByClubcardResponse response = new Activation.AccountFindByClubcardResponse()
                {
                    ContactDetailMatchStatus = accountFindResponse.ContactDetailMatchStatus,
                    Matched = accountFindResponse.Matched,
                    ErrorMessage = accountFindResponse.ErrorMessage
                };

                return response;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerActivationServiceAdapter while updating customer preferences.", ex, null);
            }
            finally
            {
                this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
            }
        }

        private string RegisterDotcomIdToCustomerAccount(string dotcomCustomerId, string clubcardNumber)
        {
            try
            {
                if (String.IsNullOrEmpty(dotcomCustomerId))
                {
                    throw new Exception("Empty dotcomid.");
                }

                long lClubcardNumber = clubcardNumber.TryParse<long>();
                if (lClubcardNumber == default(long))
                {
                    throw new Exception("Parameter clubcardNumber is mandatory and must be passed for further processing.");
                }

                this._dtStart = DateTime.UtcNow;
                AccountLinkResponse accountLinkResponse = _activationServiceClient.IGHSAccountLink(dotcomCustomerId, lClubcardNumber);
                return accountLinkResponse.ErrorMessage;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerActivationServiceAdapter while Registering customer.", ex, null);
            }
            finally
            {
                this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
            }
        }

        private bool SendActivationEmail(string toEmailId)
        {
            ClubcardOnlineServiceClient clientRegistration = null;
            this._dtStart = DateTime.UtcNow;
            try
            {
                clientRegistration = new ClubcardOnlineServiceClient();
                return clientRegistration.SendActivationEmail(toEmailId);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerActivationServiceAdapter while Sending EMail to customer.", ex, null);
            }
            finally
            {
                this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
            }
        }

        #endregion
    }
}