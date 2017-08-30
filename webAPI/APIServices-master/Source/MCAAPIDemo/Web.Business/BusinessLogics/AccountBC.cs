using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Activation=Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Security;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Tesco.ClubcardProducts.MCA.Web.Common;
using WebAPI.Contracts;
using Newtonsoft.Json;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class AccountBC
    {
        private string APIURL
        {
            get
            {
                return ConfigurationManager.AppSettings["APIURL"];
            }
        }

        APIRequester _APIRequester = null;
        
        //private CustomerServiceAdapter _customerServiceAdapter;
        //private JoinLoyaltyServiceAdapter _joinloyaltyServiceAdapter;
        //protected ClubcardServiceAdapter _clubcardServiceAdapter;
        private ConfigurationProvider _config;
        private LoggingService _logger;
        MCARequest request;
        MCAResponse response;
        Activation.CustomerVerificationDetails _CustVerificationDetails = new Activation.CustomerVerificationDetails();
        
        public AccountBC()
        {
            //this._customerServiceAdapter = new CustomerServiceAdapter();
            //this._clubcardServiceAdapter = new ClubcardServiceAdapter();
            //this._joinloyaltyServiceAdapter = joinloyaltyServiceAdapter;
            this._config = new ConfigurationProvider();
            this._logger = new LoggingService();
            _APIRequester = new APIRequester(this.APIURL);
        }

        /// <summary>
        /// Method to get the customer id
        /// </summary>
        /// <param name="clubcardNumber"></param>
        /// <param name="maxRows"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public long GetCustomerId(long clubcardNumber, int maxRows, string culture)
        {
            LogData _logData = new LogData();
            long customerID = 0;
            try
            {
                string data = "{\"service\":\"CustomerService\",\"operation\":\"GetCustomerFamilyMasterDataByClubcard\"," +
                                "\"parameters\":[{\"Key\":\"clubcardNumber\",\"Value\":\"" + clubcardNumber + "\"}," +
                                                "{\"Key\":\"maxRows\",\"Value\":\"" + maxRows + "\"}," +
                                                "{\"Key\":\"culture\",\"Value\":\"" + culture + "\"}]}";

                var apiResponse = this._APIRequester.MakeRequest(data);

                APIResponse apiResponseObj = JsonConvert.DeserializeObject<APIResponse>(apiResponse,
                                                                                new JsonSerializerSettings
                                                                                {
                                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                                });

                if (apiResponseObj.status)
                {
                    CustomerFamilyMasterData cData = JsonConvert.DeserializeObject<CustomerFamilyMasterData>(apiResponseObj.data.ToString(),
                                                                    new JsonSerializerSettings
                                                                    {
                                                                        NullValueHandling = NullValueHandling.Ignore
                                                                    });

                    customerID = (cData.CustomerData.FirstOrDefault() != null) ? cData.CustomerData.FirstOrDefault().CustomerId : 0;
                    _logData.CustomerID = customerID.ToString();
                    _logger.Submit(_logData);
                }
                else
                {
                    StringBuilder sbErrors = new StringBuilder();
                    apiResponseObj.errors.ForEach(e => sbErrors.Append(String.Format("Error - {0} - {1}", e.Key, e.Value)));
                    throw new Exception(sbErrors.ToString());
                }                
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountBC while getting CustomerID.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return customerID;
        }

        public void ParseActivationStatus(ref Activation.CustomerActivationStatusdetails activationDetails, string dotcomcustomerId, string culture)
        {
            LogData _logData = new LogData();
            _logData.RecordStep(string.Format("dotcomcustomerId: {0},culture : {1}", dotcomcustomerId, culture));
            try
            {
                Activation.CustomerActivationStatusdetails activationStatus = this.GetCustomerActivationStatusDetails(dotcomcustomerId, culture);
                activationDetails = activationStatus;
                _logData.CaptureData("activationStatus", activationStatus);
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Account BC while getting (Parse)Activation Status.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

        Activation.CustomerActivationStatusdetails GetCustomerActivationStatusDetails(string dotcomcustomerId, string culture)
        {
            LogData _logData = new LogData();
            _logData.RecordStep("dotcomid is - " + dotcomcustomerId);

            CustomerActivationStatusdetails customerActivationDetails = new CustomerActivationStatusdetails();
            request = new MCARequest();
            try
            {
                string data = "{\"service\":\"ClubcardService\",\"operation\":\"IGHSCheckCustomerActivatedStatus\"," +
                                "\"parameters\":[{\"Key\":\"dotcomCustomerID\",\"Value\":\"" + dotcomcustomerId + "\"}," +
                                                "{\"Key\":\"culture\",\"Value\":\"" + culture + "\"}]}";

                var apiResponse = this._APIRequester.MakeRequest(data);

                APIResponse apiResponseObj = JsonConvert.DeserializeObject<APIResponse>(apiResponse,
                                                                                new JsonSerializerSettings
                                                                                {
                                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                                });

                if (apiResponseObj.status)
                {
                    customerActivationDetails = JsonConvert.DeserializeObject<CustomerActivationStatusdetails>(apiResponseObj.data.ToString(),
                                                                    new JsonSerializerSettings
                                                                    {
                                                                        NullValueHandling = NullValueHandling.Ignore
                                                                    });
                    
                    _logger.Submit(_logData);
                }
                else
                {
                    StringBuilder sbErrors = new StringBuilder();
                    apiResponseObj.errors.ForEach(e => sbErrors.Append(String.Format("Error - {0} - {1}", e.Key, e.Value)));
                    throw new Exception(sbErrors.ToString());
                }  
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountBC  while getting Customer Activation Status Details.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return customerActivationDetails;
        }

        /*
        #region GetHouseholdID

        public long GetHouseholdId(long customerId, string culture)
        {
            LogData _logData = new LogData();
            _logData.RecordStep(string.Format("culture: {0}", culture));
            MCAResponse response = new MCAResponse();
            MCARequest request = new MCARequest();
            long houseHoldID = 0;
            try
            {
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_HOUSEHOLD_DETAILS_BY_CUSTOMER);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                request.Parameters.Add(ParameterNames.CULTURE, culture);
                //request and response are captured in adapter class
                response = _customerServiceAdapter.Get<List<HouseholdCustomerDetails>>(request);
                houseHoldID = (((List<HouseholdCustomerDetails>)response.Data).FirstOrDefault() != null) ? ((List<HouseholdCustomerDetails>)response.Data).FirstOrDefault().HouseHoldID : 0;
                _logData.CaptureData("houseHoldID", houseHoldID);
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AcocuntBC while getting Household ID", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            
            return houseHoldID;
        }

        #endregion

        #region GetCustomerActivationStatusDetails

        

        #endregion

        

        CustomerSecurityBlockerStatus GetCustomerVerificationStatusDetail(string customerID)
        {
            CustomerSecurityBlockerStatus custVerificationDetail = new CustomerSecurityBlockerStatus();
            request = new MCARequest();
            LogData _logData = new LogData();
                  
            try
            {
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CUSTOMER_VERIFICATION_STATUS_DETAILS);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerID);
                response = _customerServiceAdapter.Get<List<CustomerSecurityBlockerStatus>>(request);
                //request and response are captured in adapter class
                custVerificationDetail = (CustomerSecurityBlockerStatus)response.Data;
                _logger.Submit(_logData);
                return custVerificationDetail;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Account BC while getting CustomerVerification Status Details", ex, 
                    new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }

        public CustomerSecurityBlockerStatus ParseSecurityVerificationStatus(string customerID)
        {
            LogData _logData = new LogData();
            CustomerSecurityBlockerStatus custSecurityStatus = new CustomerSecurityBlockerStatus();
            try
            {
                CustomerSecurityBlockerStatus securityVerificationStatus = this.GetCustomerVerificationStatusDetail(customerID);
                if (securityVerificationStatus != null)
                {
                    _logData.RecordStep(securityVerificationStatus.JsonText());
                }
                _logger.Submit(_logData);
                return securityVerificationStatus;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Account BC while getting parse Security Verificaiton Status Details", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }

        public List<HouseholdCustomerDetails> GetHouseHoldCustomersData(long customerId, string culture)
        {
            LogData _logData = new LogData();
            List<HouseholdCustomerDetails> HouseholdcustomerDetails = new List<HouseholdCustomerDetails>();
            request = new MCARequest();

            try
            {
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_HOUSEHOLD_CUSTOMER_DATA);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                request.Parameters.Add(ParameterNames.CULTURE, culture);

                //Request and response should be logged in clubcardServiceAdapter
                response = _clubcardServiceAdapter.Get<List<HouseholdCustomerDetails>>(request);
                HouseholdcustomerDetails = (List<HouseholdCustomerDetails>)response.Data;
                if (response.Status)
                {
                    if (HouseholdcustomerDetails != null)
                    {
                        _logger.Submit(_logData);       
                        return HouseholdcustomerDetails;
                    }
                }
                _logger.Submit(_logData);       
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountBC while getting HouseHoldCustomersData.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return HouseholdcustomerDetails; // (houseHoldId, _culture);
        }

        public List<Clubcard> GetClubcardsCustomerData(long customerID, string culture)
        {
            LogData _logData = new LogData();
            List<Clubcard> CustomerClubcardDetails = new List<Clubcard>();
            request = new MCARequest();
            try
            {
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CLUBCARD_CUSTOMER_DATA);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerID);
                request.Parameters.Add(ParameterNames.CULTURE, culture);
                //request and response object should be captured in CLubcardServiceAdapter -review comment provided
                response = _clubcardServiceAdapter.Get<List<Clubcard>>(request);
                CustomerClubcardDetails = (List<Clubcard>)response.Data;
                if (response.Status)
                {
                    if (CustomerClubcardDetails != null)
                    {
                        _logger.Submit(_logData);
                        return CustomerClubcardDetails;
                    }
                }
                _logger.Submit(_logData);               
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountBC while getting ClubCardsCustomerData", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return CustomerClubcardDetails;
        }

        #region Insert Security Audit details

        public void NoteSecurityAttemptInAudit(CustomerSecurityAttemptAudit securityAttemptAudit)
        {
            LogData _logData = new LogData();
            try
            {
               
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.NOTE_SECURITY_ATTEMPT_AUDIT);
                request.Parameters.Add(ParameterNames.SECURITY_ATTEMPT_AUDIT, securityAttemptAudit);
                _customerServiceAdapter.Execute(request);
                _logger.Submit(_logData);           
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountBC while executing Security attempt in Audit", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }

        }

        #endregion Insert Security Audit details

        /// <summary>
        /// method to get the account details
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public AccountDetails GetMyAccountDetail(long customerID, string culture)
        {
            LogData _logData = new LogData();
            AccountDetails customerAccountDetails = new AccountDetails();
            MCAResponse response = new MCAResponse();
            try
            {
                MCARequest request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_MY_CUSTOMER_ACCOUNT_DETAILS);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerID);
                request.Parameters.Add(ParameterNames.CULTURE, culture);
                //request and response object should be captured in adapter class - review comment provided
                response = _clubcardServiceAdapter.Get<AccountDetails>(request);
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountBC while getting My Acocunt Details.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return response.Data as AccountDetails;
        }

        /// <summary>
        /// Method to get the configuration, Race, ISOLanguage and Province from database
        /// </summary>
        /// <param name="configurationTypes">List<DbConfigurationTypeEnum></param>
        /// <param name="culture">string</param>
        /// <returns></returns>
        public DbConfiguration GetDBConfigurations(List<DbConfigurationTypeEnum> configurationTypes, string culture)
        {
            LogData _logData = new LogData();
            _logData.RecordStep(string.Format("Culture: {0}", culture));
            DbConfiguration configuration = new DbConfiguration();
            StringBuilder configurationTypesCsv = new StringBuilder();
            try
            {
                configurationTypes.ForEach(c => configurationTypesCsv.Append((int)c + ","));
                MCARequest request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CONFIGURATIONS);
                request.Parameters.Add(ParameterNames.CONFIGURATION_TYPES, configurationTypesCsv.ToString());
                request.Parameters.Add(ParameterNames.CULTURE, culture);
                MCAResponse response = _customerServiceAdapter.Get<DBConfigurations>(request);
                configuration = response.Data as DbConfiguration;
                _logger.Submit(_logData);                
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountBC while getting DB Configurations.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return configuration;
        }

        public bool IsAccountDuplicate(CustomerFamilyMasterDataUpdate customerData)
        {
            LogData _logData = new LogData();
            bool isDuplicate = false;
            try
            {
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.CUSTOMER_DATA, customerData);
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_ACCOUNT_CONTEXT);
                response = _joinloyaltyServiceAdapter.Get<AccountContext>(request);
                AccountContext context = response.Data as AccountContext;
                //request and response captured in adapter class
                if (response.Status && context != null && (!context.IsMainAccountUnique || !context.IsAlternateAccountUnique))
                {
                    isDuplicate = true;
                }
                _logData.CaptureData("isDuplicate", isDuplicate);
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("failed in acocunBC while getting status for IsAccountDuplicate", ex,
                    new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, _logData}
                    });
            }
            return isDuplicate;
        }
         * */
    }
}
