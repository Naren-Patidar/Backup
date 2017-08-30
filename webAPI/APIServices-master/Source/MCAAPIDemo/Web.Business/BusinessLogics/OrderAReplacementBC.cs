using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.OrderReplacement;
using System.Collections;
using System;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class OrderAReplacementBC : IOrderAReplacementBC
    {
        private IServiceAdapter _clubcardServiceAdapter;
        private ILoggingService _logger;

        public OrderAReplacementBC(IServiceAdapter clubcardServiceAdapter, ILoggingService logger)
        {
            _clubcardServiceAdapter = clubcardServiceAdapter;
            _logger = logger;
        }

        public OrderAReplacementModel GetOrderAReplacementModel(long customerId, string culture)
        {
            LogData logData = new LogData();
            List<string> blackListFields = new List<string>();
            try
            {
                OrderAReplacementModel orderAReplacementModel = new OrderAReplacementModel();
                OrderReplacementStatus orderReplacementStatus = GetOrderReplacementExistingStatus(customerId, culture);
                OrderReplacementModel orderReplacement = new OrderReplacementModel();
                logData.RecordStep("Assigning values to inner OrderReplacement Model");
                orderReplacement.ClubcardNumber = orderReplacementStatus.StandardClubcardNumber;
                orderReplacement.CustomerIdEncrypt = CryptoUtility.EncryptTripleDES(customerId.ToString());
                orderReplacement.RequestType = OrderReplacementTypeEnum.NewCardAndKeyFOB;

                logData.RecordStep("Evaluating conditions");
                if (!orderReplacementStatus.OldOrderExists)
                {
                    if (orderReplacementStatus.NumOrdersPlacedInYear < 3)
                    {
                        if (orderReplacementStatus.StandardClubcardNumber != 0)
                        {
                            if (orderReplacementStatus.ClubcardTypeIndicator == BusinessConstants.ORDRPL_BOTHSTDNONSTD_CARDCODE)
                            {
                                orderAReplacementModel.divStandardNonStandard = true;
                            }
                        }
                        else
                        {
                            orderAReplacementModel.IsNonStandardCard = true;
                        }
                    }
                    else
                    {
                        orderAReplacementModel.IsMaxOrdersReached = true;
                    }
                }
                else
                {
                    orderAReplacementModel.IsInProcess = true;
                }
                orderAReplacementModel.OrderReplacementModel = orderReplacement;
                blackListFields.Add(orderAReplacementModel.OrderReplacementModel.ClubcardNumber.ToString());
                logData.BlackLists = blackListFields;
                logData.CaptureData("Old Order exists? ",orderReplacementStatus.OldOrderExists );
                _logger.Submit(logData);
                return orderAReplacementModel;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while fetching Order Replacement existing status", ex,
               new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        private OrderReplacementStatus GetOrderReplacementExistingStatus(long customerId, string culture)
        {
            LogData logData = new LogData();
            List<string> blackListFields = new List<string>();
            try
            {
                OrderReplacementStatus orderReplacementStatus = new OrderReplacementStatus();

                MCARequest request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_ORDER_REPLACEMENT_STATUS);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                request.Parameters.Add(ParameterNames.CULTURE, culture);
                
                MCAResponse response = _clubcardServiceAdapter.Get<OrderReplacementStatus>(request);
                if (response.Status)
                {
                    logData.RecordStep(string.Format("Input Parameters count for Order Replacement existing status service call: {0}", request.Parameters.Count));
                    orderReplacementStatus = (OrderReplacementStatus)response.Data;
                }
                blackListFields.Add(orderReplacementStatus.StandardClubcardNumber.ToString());
                logData.BlackLists = blackListFields;
                logData.CaptureData("Order Replacement existing status response: ", orderReplacementStatus);
                _logger.Submit(logData);
                return orderReplacementStatus;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while fetching Order Replacement existing status", ex,
               new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        public bool ProcessOrderReplacementRequest(OrderReplacementModel model)
        {
            LogData logData = new LogData();
            try
            {
                bool inProcess = false;
                MCARequest request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_PROCESS_ORDER_REPLACEMENT_REQUEST);
                request.Parameters.Add(ParameterNames.MODEL, model);
                logData.RecordStep(string.Format("Input Parameters count for Processing Order Replacement status from service call: {0}", request.Parameters.Count));
                MCAResponse response = _clubcardServiceAdapter.Get<bool>(request);
                if (response.Status)
                {
                    inProcess = (bool)response.Data;
                }
                logData.CaptureData("Process Response: ", inProcess);
                _logger.Submit(logData);
                return inProcess;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Process Request service call while fetching Order Replacement status", ex,
              new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }
    }
}
