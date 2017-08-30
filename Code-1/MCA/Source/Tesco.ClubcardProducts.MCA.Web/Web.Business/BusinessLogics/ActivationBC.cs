using System;
using System.Linq;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using ActivationService = Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerActivationServices;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public ActivationRequestStatusEnum ProcessActivationRequest(string dotcomCustomerID, long clubcardNumber, 
                                            ClubcardCustomer customerEntity)
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
                customerEntity.Clubcard.ClubcardNumber = clubcardNumber.ToString();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CLUBCARD_ACCOUNT_DETAILS);
                request.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, clubcardNumber);
                request.Parameters.Add(ParameterNames.CUSTOMER_ENTITY, customerEntity);
                request.Parameters.Add(ParameterNames.DB_CONFIGURATION, dbConfigs);                
                if (_configProvider.GetBoolAppSetting(AppConfigEnum.IsPrevActivation))
                {                    
                    if (IsPrevCard(clubcardNumber))
                    {
                        bool check2ndcall = customerEntity.FirstName.Length > 1;
                        List<Func<MCAResponse>> tasks = new List<Func<MCAResponse>>();
                        Func<MCAResponse> firstWoker = () => this._customerActivationServiceAdapter.Get<ActivationService.AccountFindByClubcardNumberResponse>(request);
                        tasks.Add(firstWoker);                        
                        _logData.RecordStep("instansiated call with full first name : " + customerEntity.FirstName);
                        if (check2ndcall)
                        {
                            MCARequest request2 = new MCARequest();
                            request2.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CLUBCARD_ACCOUNT_DETAILS);
                            request2.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, clubcardNumber);
                            ClubcardCustomer customerEntityTemp = new ClubcardCustomer(customerEntity);
                            customerEntityTemp.FirstName = customerEntity.FirstName.Substring(0, 1);

                            request2.Parameters.Add(ParameterNames.CUSTOMER_ENTITY, customerEntityTemp);
                            request2.Parameters.Add(ParameterNames.DB_CONFIGURATION, dbConfigs);
                            Func<MCAResponse> secondWorker = () => this._customerActivationServiceAdapter.Get<ActivationService.AccountFindByClubcardNumberResponse>(request2);
                            tasks.Add(secondWorker);
                            _logData.RecordStep("Call with initial latter of first name : " + customerEntityTemp.FirstName);
                        }
                        
                        List<MCAResponse> responses = new List<MCAResponse>();
                        var task = Task.Factory.StartNew(() => Parallel.ForEach<Func<MCAResponse>>(tasks, t => responses.Add(t())));
                        task.Wait();
                        _logData.RecordStep("get response from both calls");
                        _logData.CaptureData("responses", responses);
                        MCAResponse matchedRes = (from t in responses
                                                  where t != null && t.Data != null
                                                  && ((ActivationService.AccountFindByClubcardNumberResponse)t.Data).Matched
                                                  && ((ActivationService.AccountFindByClubcardNumberResponse)t.Data).ContactDetailMatchStatus.Equals("Y", StringComparison.InvariantCultureIgnoreCase)
                                                  select t).FirstOrDefault();

                        response = (matchedRes != null) ? matchedRes : responses[0];
                    }
                    else
                    {
                        response = _customerActivationServiceAdapter.Get<ActivationService.AccountFindByClubcardNumberResponse>(request);
                    }
                }
                else
                {
                    response = _customerActivationServiceAdapter.Get<ActivationService.AccountFindByClubcardNumberResponse>(request);
                }
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

        public bool IsPrevCard(long clubcard)
        {
            LogData _logData = new LogData();
            bool chk = false;
            try
            {
                DbConfigurationItem config = _configProvider.GetConfigurations(DbConfigurationTypeEnum.AppSettings, AppConfigEnum.PrevClubcardRange);
                chk = clubcard >= config.ConfigurationValue1.TryParse<long>() && clubcard <= config.ConfigurationValue2.TryParse<long>();
                _logData.RecordStep(chk ? "Prev Card" : "Non-Prev Card"); 
                _logger.Submit(_logData);
                return chk;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in ActivationBC while checking for prev card.", ex,
                    new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, _logData}
                    });
            }
        }

    }
}
