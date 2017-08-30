using System;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Collections.Generic;


namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class CustomerIdentityServiceAdapter: IServiceAdapter
    {
        private IDecryptCookieService _customerIdentityServiceClient;
        private readonly ILoggingService _logger = null;

        #region Constructors

        public CustomerIdentityServiceAdapter(IDecryptCookieService CustomerIdentityServiceClient, ILoggingService logger)
        {
            _customerIdentityServiceClient = CustomerIdentityServiceClient;
            _logger = logger;
        }

        #endregion Constructors

        #region IServiceAdapter Members

        /// <summary>
        /// Data retrieval call for CustomerIdentityService
        /// Methods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public MCAResponse Get<T>(MCARequest request)
        {
            MCAResponse res = new MCAResponse();
            LogData logData = new LogData();
            try
            {
                var operation = request.Parameters[ParameterNames.OPERATION_NAME].ToString();
                logData.RecordStep(string.Format("Executing Operation : {0}", operation));

                switch (operation)
                {
                    case OperationNames.GET_USER_IDENTITY_INFO:
                        res.Data = this.GetDecodedCookie(request.Parameters[ParameterNames.ENCODEDVALUE].TryParse<string>());
                        res.Status = true;
                        logData.RecordStep("Response received successfully. Result is passed.");
                        logData.CaptureData("Response Object", res);
                        break;
                }
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerIdentityServiceAdapter GET Method ", ex,
                   new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, logData}
                    });
            }
            return res;
        }
                
        public MCAResponse Set<T>(Common.Entities.Service.MCARequest request)
        {
            throw new NotImplementedException();
        }

        public MCAResponse Delete<T>(Common.Entities.Service.MCARequest request)
        {
            throw new NotImplementedException();
        }

        public MCAResponse Execute(Common.Entities.Service.MCARequest request)
        {
            throw new NotImplementedException();
        }

        #endregion

        private string GetDecodedCookie(string encodedText)
        {
            LogData logData = new LogData();

            try
            {
                logData.RecordStep("Calling out to decrypt cookie service");
                var custIdentity = this._customerIdentityServiceClient.GetDecodedCookie(new CustomerIdentity()
                {
                    Encodedvalue = encodedText
                });
                logData.RecordStep("Received response");

                if (custIdentity != null)
                {
                    if (!String.IsNullOrWhiteSpace(custIdentity.DecodedValue)
                                        && String.IsNullOrWhiteSpace(custIdentity.Error))
                    {
                        var elements = custIdentity.DecodedValue.Split(';');

                        if (elements.Length > 0)
                        {
                            return elements[0];
                        }
                        else
                        {
                            logData.RecordStep("Received decoded value without a semicolon");
                            return custIdentity.DecodedValue;
                        }
                    }
                    else if (!String.IsNullOrWhiteSpace(custIdentity.Error))
                    {
                        logData.RecordStep("Received error");
                        throw new Exception(custIdentity.Error);
                    }
                    else
                    {
                        throw new Exception("Decoded value is empty or blank");
                    }
                }

                logData.RecordStep("Response is null");

                return String.Empty;
            }
            finally
            {
                _logger.Submit(logData);
            }
        }
    }
}
