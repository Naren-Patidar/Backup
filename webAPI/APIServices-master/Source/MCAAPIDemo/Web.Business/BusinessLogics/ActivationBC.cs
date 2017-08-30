using System;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using ActivationService=Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerActivationServices;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Collections.Generic;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class ActivationBC : IActivationBC
    {
        IServiceAdapter _customerActivationServiceAdapter;
        private ILoggingService _logger;
        IConfigurationProvider _configProvider;

        public ActivationBC(IServiceAdapter customerActivationServiceAdapter, ILoggingService logger, IConfigurationProvider config)
        {
            this._customerActivationServiceAdapter = customerActivationServiceAdapter;
            this._logger = logger;
            this._configProvider = config;
        }

        public ActivationRequestStatusEnum ProcessActivationRequest(string dotcomCustomerID, long clubcardNumber, ClubcardCustomer customerEntity)
        {
            LogData _logData = new LogData();           
            ActivationRequestStatusEnum status;
            MCARequest request;
            MCAResponse response;
            try
            {
                DBConfigurations dbConfigs = _configProvider.GetConfigurations(DbConfigurationTypeEnum.Activation);
                _logData.CaptureData("dbConfigs", dbConfigs);
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CLUBCARD_ACCOUNT_DETAILS);
                request.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, clubcardNumber);
                request.Parameters.Add(ParameterNames.CUSTOMER_ENTITY, customerEntity);
                request.Parameters.Add(ParameterNames.DB_CONFIGURATION, dbConfigs);
                response = _customerActivationServiceAdapter.Get<ActivationService.AccountFindByClubcardNumberResponse>(request);
                _logData.CaptureData("response", response);
                ActivationService.AccountFindByClubcardNumberResponse accountFindResponse = (ActivationService.AccountFindByClubcardNumberResponse)response.Data;
                if (!string.IsNullOrEmpty(accountFindResponse.ErrorMessage))
                {
                    status = ActivationRequestStatusEnum.ErrorMessage;
                    _logData.RecordStep(string.Format("status {0}:", status));
                }
                else
                {
                    //check if address provided by customer is exists in the Clubcard system
                    // check if Clubcard number exists in the Clubcard system
                    if (!accountFindResponse.Matched || accountFindResponse.ContactDetailMatchStatus.ToString().ToUpper() != "Y")
                    {
                        status = ActivationRequestStatusEnum.ClubcardDetailsDoesntMatch;
                        _logData.RecordStep(string.Format("status {0}:", status));
                    }
                    else
                    {
                        // if clubcard number and address matched in the Clubcard system, then allows the dotcom customer to link 
                        // with the Clubcard system by calling the Registration service.
                        request = new MCARequest();
                        request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.REGISTER_DOTCOMID_CUSTOMERACCOUNT);
                        request.Parameters.Add(ParameterNames.DOTCOM_CUSTOMER_ID, dotcomCustomerID);
                        request.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, clubcardNumber);
                        response = _customerActivationServiceAdapter.Get<ActivationService.AccountLinkResponse>(request);
                        _logData.CaptureData("response", response);
                        ActivationService.AccountLinkResponse registerResponse = (ActivationService.AccountLinkResponse)response.Data;

                        if (string.IsNullOrEmpty(registerResponse.ErrorMessage))
                        {
                            status = ActivationRequestStatusEnum.ActivationSuccessful;
                            _logData.RecordStep(string.Format("Activation Successful status {0}:", status));
                        }
                        else if (registerResponse.ErrorMessage.Contains("DuplicateDotcomID"))
                        {
                            status = ActivationRequestStatusEnum.DuplicateDotcomID;
                            _logData.RecordStep(string.Format("Duplicate DotcomID status {0}:", status));
                        }
                        else if (registerResponse.ErrorMessage.Contains("CustomerID already"))
                        {
                            status = ActivationRequestStatusEnum.CustomerIDalready;
                            _logData.RecordStep(string.Format("CustomerID already status {0}:", status));
                        }
                        else
                        {
                            status = ActivationRequestStatusEnum.ActivationFailed;
                            _logData.RecordStep(string.Format("ActivationFailed status {0}:", status));
                        }
                    }
                }
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in ActivationBC while Processing Activation Request.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return status;
        }
    }
}
