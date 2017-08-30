using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.ServiceModel;
using System.Collections.Generic;
using System.Collections;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.SmartVoucherServices;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.ClubcardService;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Points;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class SmartVoucherServiceAdapter : IServiceAdapter
    {
        ISmartVoucherServices _smartVoucherServiceClient = null;
        private readonly ILoggingService _logger = null;

        public SmartVoucherServiceAdapter(ISmartVoucherServices smartVoucherServiceClient, ILoggingService logger)
        {
            _smartVoucherServiceClient = smartVoucherServiceClient;
            _logger = logger;
        }

        #region IServiceAdapter Members

        public MCAResponse Get<T>(MCARequest req)
        {
            MCAResponse res = new MCAResponse();
            LogData logData = new LogData();
            try
            {
                logData.CaptureData("Request Object", req);
                var operation = req.Parameters[ParameterNames.OPERATION_NAME].ToString();
                logData.CaptureData("Operation:", operation);

                switch (operation)
                {
                    case OperationNames.GET_UNUSED_VOUCHER_DETAILS:
                        if (req.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) &&
                            req.Parameters.Keys.Contains(ParameterNames.CLUBCARD_NUMBER) &&
                            req.Parameters.Keys.Contains(ParameterNames.CULTURE))
                        {
                            res.Data = this.GetUnusedVoucherDetails(
                                                    Convert.ToInt64(req.Parameters[ParameterNames.CUSTOMER_ID]),
                                                    Convert.ToInt64(req.Parameters[ParameterNames.CLUBCARD_NUMBER]),
                                                    req.Parameters[ParameterNames.CULTURE].ToString());
                            logData.RecordStep("Response received successfully");
                            res.Status = true;
                        }
                        break;
                    case OperationNames.GET_VOUCHER_REWARD_DETAILS:
                        if (req.Parameters.Keys.Contains(ParameterNames.CLUBCARD_NUMBER))
                        {
                            res.Data = this.GetVoucherRewardDetails(Convert.ToInt64(req.Parameters[ParameterNames.CLUBCARD_NUMBER]));
                            logData.RecordStep("Response received successfully");
                            res.Status = true;
                        }
                        break;
                    case OperationNames.GET_USED_VOUCHER_DETAILS:
                        if (req.Parameters.Keys.Contains(ParameterNames.CLUBCARD_NUMBER))
                        {
                            res.Data = this.GetUsedVoucherDetails(req.Parameters[ParameterNames.CLUBCARD_NUMBER].ToString());
                            logData.RecordStep("Response received successfully");
                            res.Status = true;
                        }
                        break;
                    case OperationNames.GET_REWARD_DETAILS_MILES:
                        if (req.Parameters.Keys.Contains(ParameterNames.CLUBCARD_NUMBER) &&
                                                req.Parameters.Keys.Contains(ParameterNames.REASON_CODE))
                        {
                            res.Data = this.GetRewardDetailsMiles(req.Parameters[ParameterNames.CLUBCARD_NUMBER].ToString(),
                                                        Convert.ToInt32(req.Parameters[ParameterNames.REASON_CODE]));
                            logData.RecordStep("Response received successfully");
                            res.Status = true;
                        }
                        break;
                    case OperationNames.GET_CUSTOMERVOUCHERVAL_CPS:
                        res.Data = this.GetCustomerVoucherValCPS(req.Parameters[ParameterNames.CLUBCARD_NUMBER].ToString(), req.Parameters[ParameterNames.START_DATE].ToString(), req.Parameters[ParameterNames.END_DATE].ToString());
                            logData.RecordStep("Response received successfully");
                        res.Status = true;
                        break;
                }
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                ex.Data.Add("Get():InputParam", req);
                res.Status = false;
                res.ErrorMessage = ex.Message;

                throw GeneralUtility.GetCustomException("Failed in Smart Voucher Service Adapter GET", ex,
                   new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, logData}
                    });
            }
            
            return res;
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
            throw new NotImplementedException();
        }

        #endregion IServiceAdapter Members

        #region Private Members

        private List<VoucherRewardDetails> GetVoucherRewardDetails(long clubcardNumber)
        {
            LogData logData = new LogData();

            try
            {
                VoucherRewardDetailsList voucherDetails = new VoucherRewardDetailsList();
                logData.RecordStep("Calling GetRewardDtls");
                GetRewardDtlsRsp response = _smartVoucherServiceClient.GetRewardDtls(clubcardNumber.ToString());
                logData.CaptureData("Reward Details response ", response);
                if (response != null && response.dsResponse != null && response.dsResponse.Tables != null && response.dsResponse.Tables.Count > 0)
                {
                    voucherDetails.ConvertFromDataset(response.dsResponse);
                    logData.CaptureData("Voucher Details ", voucherDetails);
                }
                _logger.Submit(logData);
                return voucherDetails.VoucherRewardDetailList;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting status for GetRewardDetails call ", ex,
                    new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// Method to get the Unused voucher details for the customer
        /// </summary>
        /// <param name="customerID">long customerID</param>
        /// <param name="cardNumber">long cardNumber</param>
        /// <param name="culture">string culture</param>
        /// <returns></returns>
        private List<VoucherDetails> GetUnusedVoucherDetails(long customerID, long cardNumber, string culture)
        {
            LogData logData = new LogData();
            List<VoucherDetails> voucherDetailsList = new List<VoucherDetails>();

            try
            {
                GetUnusedVoucherDtlsRsp response = _smartVoucherServiceClient.GetUnusedVoucherDtls(cardNumber.ToString());
                logData.CaptureData("Unused Vouchers Details Response", response);
                if (response != null && response.dsResponse != null && response.dsResponse.Tables != null && response.dsResponse.Tables.Count > 0)
                {
                    VoucherDetailsList lstvoucherDetailsList = new VoucherDetailsList();
                    lstvoucherDetailsList.ConvertFromDataset(response.dsResponse);
                    voucherDetailsList = lstvoucherDetailsList.VoucherDetailsListInstance;
                }
                _logger.Submit(logData);
                return voucherDetailsList;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting status for GetUnusedVoucherDtls call ", ex,
                    new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// Method to get the Rewards and Points for the collection period
        /// </summary>
        /// <param name="cardNumber">string cardNumber</param>
        /// <param name="stDate">string Collection Period Start Date</param>
        /// <param name="enDate">string Collection Period End Date</param>
        /// <returns></returns>
        private List<RewardAndPoints> GetCustomerVoucherValCPS(string cardNumber, string stDate, string enDate)
        {
           
            LogData logData = new LogData();
            List<RewardAndPoints> rewarAndPointsDetailList = new List<RewardAndPoints>();
            GetVoucherValAllCPSRsp response = new GetVoucherValAllCPSRsp();
            try
            {
                //Get the Clubcard voucher value from the webservice.                    
                response = _smartVoucherServiceClient.GetVoucherValCPS(cardNumber, stDate, enDate);
                logData.CaptureData("Customer Voucher Value Response", response);
                if (response != null && response.dsResponse != null && response.dsResponse.Tables != null && response.dsResponse.Tables.Count > 0)
                {
                    RewardAndPointsLst lstRewardAndPoints = new RewardAndPointsLst();
                    lstRewardAndPoints.ConvertFromDataset(response.dsResponse);
                    rewarAndPointsDetailList = lstRewardAndPoints.RewardAndPointsLstInstance;
                }
                _logger.Submit(logData);
                return rewarAndPointsDetailList;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting status for GetVoucherValCPS call ", ex,
                    new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// Method to close the srvice client object in case there is any fault in service client
        /// </summary>
        /// <param name="smartVoucherServicesClient">SmartVoucherServicesClient object</param>
        private void CloseServiceConnection(ISmartVoucherServices smartVoucherServicesClient)
        {
            var serviceClient = smartVoucherServicesClient as ClientBase<ISmartVoucherServices>;

            if (serviceClient != null)
            {
                if (serviceClient.State == CommunicationState.Faulted)
                {
                    serviceClient.Abort();
                }
               
            }
        }

        /// <summary>
        /// Method to get the used vouchers details for the customer
        /// </summary>
        /// <param name="clubcardNumber">string clubcardNumber</param>
        /// <returns></returns>
        private List<VoucherUsageSummary> GetUsedVoucherDetails(string clubcardNumber)
        {
            LogData logData = new LogData();            
            List<VoucherUsageSummary> voucherRewardDetailsList = new List<VoucherUsageSummary>();
            VoucherUsageSummaryLst lstvoucherRewardDetailsList = new VoucherUsageSummaryLst();

            try
            {
                GetUsedVoucherDtlsRsp response = _smartVoucherServiceClient.GetUsedVoucherDtls(clubcardNumber);
                logData.CaptureData("Used Vouchers Details Response", response);
                if (response != null && response.dsResponse != null && response.dsResponse.Tables != null && response.dsResponse.Tables.Count > 0)
                {
                    lstvoucherRewardDetailsList.ConvertFromDataset(response.dsResponse);
                    voucherRewardDetailsList = lstvoucherRewardDetailsList.VoucherUsageSummaryLstInstance;
                }
               
                _logger.Submit(logData);

                return voucherRewardDetailsList;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting status for GetUsedVoucherDtls call ", ex,
                       new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        public List<MilesRewardDetails> GetRewardDetailsMiles(string clubcardNumber, int reasonCode)
        {
            LogData logData = new LogData();

            List<MilesRewardDetails> milesRewardDetails = new List<MilesRewardDetails>();
            MilesRewardDetailsLst lstMilesRewardDetails = new MilesRewardDetailsLst();
            try
            {
                GetRewardDtlsMilesRsp response = _smartVoucherServiceClient.GetRewardDtlsMiles(clubcardNumber, reasonCode);
                logData.CaptureData("Used Vouchers Details Response", response);
                if (response != null && response.dsResponse != null && response.dsResponse.Tables != null && response.dsResponse.Tables.Count > 0)
                {
                    lstMilesRewardDetails.ConvertFromDataset(response.dsResponse);
                    milesRewardDetails = lstMilesRewardDetails.MilesRewardDetailsLstInstance;
                }

                _logger.Submit(logData);
                return milesRewardDetails;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting status for GetRewardDtlsMiles call ", ex,
                    new Dictionary<string, object>() 
                    {
                        { LogConfigProvider.EXCLOGDATAKEY, logData }
                    });
            }
        }

        #endregion
    }
}