using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
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
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Identity;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class AccountBC : IAccountBC
    {
        private IServiceAdapter _customerServiceAdapter;
        private IServiceAdapter _joinloyaltyServiceAdapter;
        protected IServiceAdapter _clubcardServiceAdapter;
        private IServiceAdapter _identityServiceAdaptor;
        private IConfigurationProvider _config;
        private ILoggingService _logger;
        private IServiceAdapter _CustomerIdentityServiceAdapter;
        MCARequest request;
        MCAResponse response;
        Activation.CustomerVerificationDetails _CustVerificationDetails = new Activation.CustomerVerificationDetails();

        public AccountBC(   IServiceAdapter customerServiceAdapter, 
                            IServiceAdapter clubcardServiceAdapter, 
                            IServiceAdapter joinloyaltyServiceAdapter, 
                            IServiceAdapter customerIdentityServiceAdapter, 
                            IServiceAdapter identityService,
                            IConfigurationProvider config, 
                            ILoggingService logger)
        {
            this._customerServiceAdapter            = customerServiceAdapter;
            this._identityServiceAdaptor            = identityService;
            this._clubcardServiceAdapter            = clubcardServiceAdapter;
            this._joinloyaltyServiceAdapter         = joinloyaltyServiceAdapter;
            this._CustomerIdentityServiceAdapter    = customerIdentityServiceAdapter;
            this._config                            = config;
            this._logger                            = logger;
        }

        public AccountBC()
        {
            this._customerServiceAdapter            = ServiceLocator.Current.GetInstance<IServiceAdapter>();
            this._identityServiceAdaptor            = ServiceLocator.Current.GetInstance<IServiceAdapter>();
            this._clubcardServiceAdapter            = ServiceLocator.Current.GetInstance<IServiceAdapter>();
            this._joinloyaltyServiceAdapter         = ServiceLocator.Current.GetInstance<IServiceAdapter>();
            this._CustomerIdentityServiceAdapter    = ServiceLocator.Current.GetInstance<IServiceAdapter>();
            this._identityServiceAdaptor            = ServiceLocator.Current.GetInstance<IServiceAdapter>();
            this._config                            = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
            this._logger                            = ServiceLocator.Current.GetInstance<ILoggingService>();
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
                MCAResponse response = new MCAResponse();
                MCARequest request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CUSTOMER_FAMILY_BY_CLUBCARD);
                request.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, clubcardNumber);
                request.Parameters.Add(ParameterNames.MAX_ROWS, maxRows);
                request.Parameters.Add(ParameterNames.CULTURE, culture);
                response = _customerServiceAdapter.Get<CustomerFamilyMasterData>(request);
                customerID = (((CustomerFamilyMasterData)response.Data).CustomerData.FirstOrDefault() != null) ? ((CustomerFamilyMasterData)response.Data).CustomerData.FirstOrDefault().CustomerId : 0;
                _logData.CustomerID = customerID.ToString();
                _logger.Submit(_logData);
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

        public long GetHouseholdId(long customerID, string culture)
        {
            LogData _logData = new LogData();
            _logData.RecordStep(string.Format("culture: {0}", culture));
            MCAResponse response = new MCAResponse();
            MCARequest request = new MCARequest();
            long houseHoldID = 0;
            try
            {
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_HOUSEHOLD_DETAILS_BY_CUSTOMER);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerID);
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

        public void ParseActivationStatus(ref Activation.CustomerActivationStatusdetails activationDetails, string dotcomcustomerId,
            string culture)
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

        public List<HouseholdCustomerDetails> GetHouseHoldCustomersData(long customerID, string culture)
        {
            LogData _logData = new LogData();
            List<HouseholdCustomerDetails> HouseholdcustomerDetails = new List<HouseholdCustomerDetails>();
            request = new MCARequest();

            try
            {
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_HOUSEHOLD_CUSTOMER_DATA);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerID);
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
                if (_config.GetBoolAppSetting(AppConfigEnum.AccountDuplicateCheckRequired))
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

        public ValidationDetails ValidateToken(string oAuthToken)
        {
            LogData _logData = new LogData();
            ValidationDetails response = new ValidationDetails();
            try
            {
                MCARequest request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.VALIDATE_TOKEN);
                request.Parameters.Add(ParameterNames.OAUTH_TOKEN, oAuthToken);                
                MCAResponse res = _identityServiceAdaptor.Execute(request);
                if (res.Status)
                {
                    response = res.Data as ValidationDetails;
                }
                
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountBC while trying to validate the oauth access token.", ex,
                    new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, _logData}
                    });
            }
            return response;
        }

        public MCAServiceTokenRes GetMCAServiceToken()
        {
            LogData logData = new LogData();
            MCAServiceTokenRes response = new MCAServiceTokenRes();
            try
            {
                MCARequest request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.MCASERVICE_TOKEN);                
                MCAResponse res = _identityServiceAdaptor.Execute(request);
                if (res.Status)
                {
                    response = res.Data as MCAServiceTokenRes;
                }
                
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("failed in Account BC while getting MCA service token", ex,
                    new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, logData}
                    });
            }
            return response;
        }

        public LegacyLookupRes GetDotComIDFromLookup(string mcaServiceToken, string uuid)
        {
            LogData _logData = new LogData();
            LegacyLookupRes response = new LegacyLookupRes();
            try
            {
                MCARequest request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.LEGACY_LOOKUP);
                request.Parameters.Add(ParameterNames.MCASERVICE_TOKEN, mcaServiceToken);
                request.Parameters.Add(ParameterNames.UUID, uuid);

                MCAResponse res = this._identityServiceAdaptor.Execute(request);
                if (res.Status)
                {
                    response = res.Data as LegacyLookupRes;
                }

                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountBC while getting dotcomid", ex,
                    new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, _logData}
                    });
            }
            return response;
        }

        public string GetIdentityInfomation(string encodedvalue)
        {
            LogData _logData = new LogData();

            try
            {
                _logData.CaptureData("Encodedvalue", encodedvalue.ToString());

                request = new MCARequest();

                request.Parameters.Add(ParameterNames.ENCODEDVALUE, encodedvalue);
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_USER_IDENTITY_INFO);
                response = _CustomerIdentityServiceAdapter.Get<string>(request);

                _logger.Submit(_logData);

                return response.Data as string;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountBC while getting status for GetIdentityInfomation", ex,
                    new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, _logData}
                    });
            }
        }

        Activation.CustomerActivationStatusdetails GetCustomerActivationStatusDetails(string dotcomcustomerId, string culture)
        {
            LogData _logData = new LogData();

            Activation.CustomerActivationStatusdetails customerActivationDetails = new Activation.CustomerActivationStatusdetails();
            request = new MCARequest();
            try
            {
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CUSTOMER_ACTIVATION_STATUS_DETAILS);
                request.Parameters.Add(ParameterNames.DOTCOM_CUSTOMER_ID, dotcomcustomerId);
                request.Parameters.Add(ParameterNames.CULTURE, culture);

                response = _clubcardServiceAdapter.Get<Activation.CustomerActivationStatusdetails>(request);
                //request and response will be captured in adapter class-review comment provided for logging
                if (response.Status)
                {
                    customerActivationDetails = response.Data as Activation.CustomerActivationStatusdetails;
                    if (customerActivationDetails != null)
                    {
                        return customerActivationDetails;
                    }
                }
                _logger.Submit(_logData);
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
    }
}
