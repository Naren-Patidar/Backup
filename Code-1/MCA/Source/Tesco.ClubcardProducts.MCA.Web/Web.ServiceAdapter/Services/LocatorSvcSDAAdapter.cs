using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Data;
using System.Xml;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class LocatorSvcSDAAdapter : IServiceAdapter
    {
        private readonly ILoggingService _logger = null;

        #region Constructors

        private ILocatorSvcSDA _locatorServiceClient;

        public LocatorSvcSDAAdapter(ILocatorSvcSDA locatorServiceClient, ILoggingService logger)
        {
            _locatorServiceClient = locatorServiceClient;
            _logger = logger;
        }

        #endregion
        #region IServiceAdapter Members

        /// <summary>
        /// Data retrieval call for Join Loyalty Service
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

                logData.CaptureData("Request object", request);
                var operation = request.Parameters[ParameterNames.OPERATION_NAME].ToString();

                switch (operation)
                {
                    case OperationNames.GET_ADDRESSES_FOR_POST_CODE:
                        if (request.Parameters.Keys.Contains(ParameterNames.POST_CODE))
                        {
                            res.Data = GetAddressesForPostCodeList((request.Parameters[ParameterNames.POST_CODE]).TryParse<string>());
                            res.Status = true;
                            logData.RecordStep("Success for object");
                        }
                        break;
                }
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.ErrorMessage = ex.Message;
                throw GeneralUtility.GetCustomException("Failed in GET Operation method", ex,
                             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
            return res;
        }

        /// <summary>
        /// Data update call of Join Loyalty Service methods
        /// returning boolean value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
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
        #region Private Methods

        private List<AddressByPostCode> GetAddressesForPostCodeList(string postCode)
        {
            AddressByPostCodeList lstAddressByPostCode = new AddressByPostCodeList();
            string resultAddress = "";
            LogData logData = new LogData();

            try
            {
                resultAddress = _locatorServiceClient.FindAddressLite(postCode, null, null);
                logData.CaptureData(string.Format("ResultAddress for postcode {0}", postCode), resultAddress);
                if (resultAddress != string.Empty)
                {
                    lstAddressByPostCode.ConvertFromXml(resultAddress);
                }
                _logger.Submit(logData);
                return lstAddressByPostCode.AddressByPostCodeLists;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Get Address for postcode address list" , ex,
             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }


        #endregion

    }
}

