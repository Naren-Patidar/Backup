using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Identity;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Tesco.ClubcardProducts.MCA.Web.RestClient;
using Tesco.ClubcardProducts.MCA.Web.RestClient.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Newtonsoft.Json;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Enterprise
{
    public class IdentityApiAdapter : IServiceAdapter
    {
        private readonly ILoggingService _logger = null;
        private readonly IConfigurationProvider _config = null;
        private readonly IRestProxies _restClientManager = null;

        #region Constructors

        public IdentityApiAdapter(IConfigurationProvider config, ILoggingService logger, IRestProxies restproxies)
        {
            _logger = logger;
            _config = config;
            _restClientManager = restproxies;
        }

        #endregion

        #region Properties

        public Dictionary<string, string> IdentityEndPoints 
        {
            get
            {
                string identityEPs = this._config.GetStringAppSetting(AppConfigEnum.IdentityEndPoints);
                if (String.IsNullOrWhiteSpace(identityEPs))
                {
                    return new Dictionary<string, string>();
                }
                else
                {
                    return JsonConvert.DeserializeObject<Dictionary<string, string>>(identityEPs);
                }
            }
        }

        public string ValidateTokenURL 
        { 
            get 
            {
                return this.IdentityEndPoints["validatetoken"];
            } 
        }

        public string ServiceTokenURL
        {
            get
            {
                return this.IdentityEndPoints["servicetoken"];
            }
        }

        public string LegacyLookupURL
        {
            get
            {
                return this.IdentityEndPoints["legacylookup"];
            }
        }

        #endregion Properties

        #region IServiceAdapter Members

        public MCAResponse Get<T>(MCARequest request)
        {
            throw new NotImplementedException();
        }

        public MCAResponse Set<T>(MCARequest request)
        {
            throw new NotImplementedException();
        }

        public MCAResponse Delete<T>(MCARequest request)
        {
            throw new NotImplementedException();
        }

        public MCAResponse Execute(MCARequest request)
        {
            MCAResponse response = new MCAResponse();
            string operation = request.Parameters.ContainsKey(ParameterNames.OPERATION_NAME) ? request.Parameters[ParameterNames.OPERATION_NAME].ToString() : string.Empty;
            switch (operation)
            {
                case OperationNames.VALIDATE_TOKEN:
                    response.Data = this.ValidateToken(request.Parameters[ParameterNames.OAUTH_TOKEN].TryParse<string>());
                    response.Status = true;
                    break;
                case OperationNames.MCASERVICE_TOKEN:
                    response.Data = this.GetMCAServiceToken();
                    response.Status = true;
                    break;
                case OperationNames.LEGACY_LOOKUP:
                    response.Data = this.GetDotcomIDFromLegacyLookUp(request.Parameters[ParameterNames.MCASERVICE_TOKEN].TryParse<string>(), request.Parameters[ParameterNames.UUID].TryParse<string>());
                    response.Status = true;
                    break;
            }
            return response;
        }

        #endregion

        /// <summary>
        /// Method to validate the oAuthToken
        /// </summary>
        /// <param name="oAuthToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public ValidationDetails ValidateToken(string oAuthToken)
        {
            LogData logData = new LogData();
            logData.RecordStep(String.Format("oAuttoken - {0}", oAuthToken));

            try
            {
                string identityClientId = DBConfigurationManager.Instance.GetSecureConfigItem(SecureConfigEnum.MCAClientID.ToString());
                var url = Helper.AppendUrlQueryString(this.ValidateTokenURL,
                                            new List<KeyValuePair<string, string>> 
                                            { 
                                                new KeyValuePair<string, string>(EnterpriceServiceSettingConst.Access_Token, oAuthToken), 
                                                new KeyValuePair<string, string>(EnterpriceServiceSettingConst.Client_ID, identityClientId) 
                                            });

                logData.RecordStep(String.Format("validate token url - {0}", url));
                var response = _restClientManager.RestGet<ValidationDetails>(url, string.Empty);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        this._logger.Submit(logData);
                        return response.Body;
                    default:
                        logData.RecordStep(response.JsonText());
                        throw new Exception("Validate Token Failure.");
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception while getting Validation details from identity service", ex,
                 new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// Method to get MCA service token 
        /// </summary>
        /// <param name="oAuthToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public MCAServiceTokenRes GetMCAServiceToken()
        {
            LogData logData = new LogData();
           
            try
            {
                var requestData = new MCAServiceTokenReq()
                {
                    username = DBConfigurationManager.Instance.GetSecureConfigItem(SecureConfigEnum.MCAServiceTokenUsername.ToString()),
                    password = DBConfigurationManager.Instance.GetSecureConfigItem(SecureConfigEnum.MCAServiceTokenPassword.ToString()),
                    grant_type = EnterpriceServiceSettingConst.Password_GrantType,
                    client_id = DBConfigurationManager.Instance.GetSecureConfigItem(SecureConfigEnum.MCAClientID.ToString()),
                    scope = EnterpriceServiceSettingConst.service
                };

                var url = this.ServiceTokenURL;

                logData.RecordStep(String.Format("Service Token URL - {0}", url));
                var response = _restClientManager.RestPost<MCAServiceTokenRes, MCAServiceTokenReq>(url, requestData, string.Empty);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        this._logger.Submit(logData);
                        logData.CaptureData("Service Token Response", response.Body);
                        return response.Body;
                    default:
                        logData.RecordStep(response.JsonText());
                        throw new Exception("Service Token Failure.");
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception while getting MCA Service token details from identity service - token method", ex,
                 new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mcaServiceToken"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public LegacyLookupRes GetDotcomIDFromLegacyLookUp(string mcaServiceToken, string uuid)
        {
            LogData logData = new LogData();
            
            try
            {
                logData.CaptureData("mcaServiceToken", mcaServiceToken);
                logData.RecordStep("uuid:" + uuid);

                var requestData = new LegacyLookupReq()
                {
                    client_id = DBConfigurationManager.Instance.GetSecureConfigItem(SecureConfigEnum.MCAClientID.ToString()),
                    uuid = String.Format("{0}{1}", EnterpriceServiceSettingConst.UUID_PREFIX, uuid),
                    fields = new List<string>() {
                    {
                        ParameterNames.DXSH_CUSTOMERID
                    }}
                };

                logData.CaptureData("RequestData", requestData.JsonText());

                var response = this._restClientManager.RestPost<LegacyLookupRes, LegacyLookupReq>(
                                            this.LegacyLookupURL, 
                                            requestData, 
                                            String.Format("{0} {1}", EnterpriceServiceSettingConst.BEARER, mcaServiceToken));

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        this._logger.Submit(logData);
                        logData.CaptureData("Legacy Lookup Response", response.Body);
                        return response.Body;
                    default:
                        logData.RecordStep(response.JsonText());
                        throw new Exception(String.Format("Legacy Lookup Failure. Errored Response - {0}", response.JsonText()));
                }

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception while getting Dotcom ID from identity legacy lookup ", ex,
                 new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

    }
}