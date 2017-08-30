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
using Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder;
using Tesco.ClubcardProducts.MCA.Web.Common;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class SmartVoucherServiceAdapter : IServiceAdapter
    {
        ISmartVoucherServices _smartVoucherServiceClient = new SmartVoucherServicesClient();
        Recorder _recorder = null;

        public SmartVoucherServiceAdapter(Recorder recorder)
        {
            this._recorder = recorder;
        }

        #region IServiceAdapter Members

        public MCAResponse Get(MCARequest req)
        {
            MCAResponse res = new MCAResponse();

            var operation = req.Parameters[ParameterNames.OPERATION_NAME].ToString();

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
                        res.Status = true;
                    }
                    break;
                case OperationNames.GET_VOUCHER_REWARD_DETAILS:
                    if (req.Parameters.Keys.Contains(ParameterNames.CLUBCARD_NUMBER))
                    {
                        res.Data = this.GetVoucherRewardDetails(Convert.ToInt64(req.Parameters[ParameterNames.CLUBCARD_NUMBER]));
                        res.Status = true;
                    }
                    break;
                case OperationNames.GET_USED_VOUCHER_DETAILS:
                    if (req.Parameters.Keys.Contains(ParameterNames.CLUBCARD_NUMBER))
                    {
                        res.Data = this.GetUsedVoucherDetails(req.Parameters[ParameterNames.CLUBCARD_NUMBER].ToString());
                        res.Status = true;
                    }
                    break;
                case OperationNames.GET_REWARD_DETAILS_MILES:
                    if (req.Parameters.Keys.Contains(ParameterNames.CLUBCARD_NUMBER) &&
                                            req.Parameters.Keys.Contains(ParameterNames.REASON_CODE))
                    {
                        res.Data = this.GetRewardDetailsMiles(req.Parameters[ParameterNames.CLUBCARD_NUMBER].ToString(),
                                                    Convert.ToInt32(req.Parameters[ParameterNames.REASON_CODE]));
                        res.Status = true;
                    }
                    break;
                case OperationNames.GET_CUSTOMERVOUCHERVAL_CPS:
                    res.Data = this.GetCustomerVoucherValCPS(req.Parameters[ParameterNames.CLUBCARD_NUMBER].ToString(), req.Parameters[ParameterNames.START_DATE].ToString(), req.Parameters[ParameterNames.END_DATE].ToString());
                    res.Status = true;
                    break;
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
            VoucherRewardDetailsList voucherDetails = new VoucherRewardDetailsList();

            GetRewardDtlsRsp response = _smartVoucherServiceClient.GetRewardDtls(clubcardNumber.ToString());

            this._recorder.RecordResponse(new RecordLog { Result = response.JsonText() },
                Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.SmartVoucherServices.ToString(),
                "GetRewardDtls", ResponseType.js);

            if (response != null)
            {
                voucherDetails.ConvertFromDataset(response.dsResponse);
                if (response.ErrorMessage != null)
                {
                    throw new Exception(response.ErrorMessage);
                }
            }
            return voucherDetails.VoucherRewardDetailList;
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
            var voucherDetailsList = new List<VoucherDetails>();

            GetUnusedVoucherDtlsRsp response = _smartVoucherServiceClient.GetUnusedVoucherDtls(cardNumber.ToString());

            this._recorder.RecordResponse(new RecordLog { Result = response.JsonText() },
                Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.SmartVoucherServices.ToString(),
                "GetUnusedVoucherDtls", ResponseType.js);

            if (response != null && response.dsResponse != null)
            {
                if (response.ErrorMessage != null)
                {
                    throw new Exception(response.ErrorMessage);
                }
                VoucherDetailsList lstvoucherDetailsList = new VoucherDetailsList();
                lstvoucherDetailsList.ConvertFromDataset(response.dsResponse);
                voucherDetailsList = lstvoucherDetailsList.VoucherDetailsListInstance;
            }
            return voucherDetailsList;
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
            List<RewardAndPoints> rewarAndPointsDetailList = new List<RewardAndPoints>();
            GetVoucherValAllCPSRsp response = new GetVoucherValAllCPSRsp();
            try
            {
                //Get the Clubcard voucher value from the webservice.                    
                response = _smartVoucherServiceClient.GetVoucherValCPS(cardNumber, stDate, enDate);

                this._recorder.RecordResponse(new RecordLog { Result = response.JsonText() },
                    Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.SmartVoucherServices.ToString(), 
                    "GetVoucherValCPS", ResponseType.js);

                if (response != null && response.dsResponse != null)
                {
                    RewardAndPointsLst lstRewardAndPoints = new RewardAndPointsLst();
                    lstRewardAndPoints.ConvertFromDataset(response.dsResponse);
                    rewarAndPointsDetailList = lstRewardAndPoints.RewardAndPointsLstInstance;
                }
                return rewarAndPointsDetailList;
            }
            catch (Exception ex)
            {
                throw ex;
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
            List<VoucherUsageSummary> voucherRewardDetailsList = new List<VoucherUsageSummary>();
            VoucherUsageSummaryLst lstvoucherRewardDetailsList = new VoucherUsageSummaryLst();

            GetUsedVoucherDtlsRsp response = _smartVoucherServiceClient.GetUsedVoucherDtls(clubcardNumber);

            this._recorder.RecordResponse(new RecordLog { Result = response.JsonText() },
                Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.SmartVoucherServices.ToString(),
                "GetUsedVoucherDtls", ResponseType.js);

            if (response != null)
            {
                if (response.ErrorMessage != null)
                {
                    throw new Exception(response.ErrorMessage);
                }
                lstvoucherRewardDetailsList.ConvertFromDataset(response.dsResponse);
                voucherRewardDetailsList = lstvoucherRewardDetailsList.VoucherUsageSummaryLstInstance;
            }

            return voucherRewardDetailsList;
        }

        public List<MilesRewardDetails> GetRewardDetailsMiles(string clubcardNumber, int reasonCode)
        {
            List<MilesRewardDetails> milesRewardDetails = new List<MilesRewardDetails>();
            MilesRewardDetailsLst lstMilesRewardDetails = new MilesRewardDetailsLst();
            GetRewardDtlsMilesRsp response = _smartVoucherServiceClient.GetRewardDtlsMiles(clubcardNumber, reasonCode);

            this._recorder.RecordResponse(new RecordLog { Result = response.JsonText() },
                Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.SmartVoucherServices.ToString(),
                "GetRewardDtlsMiles", ResponseType.js);

            if (response != null)
            {
                if (response.ErrorMessage != null)
                {
                    throw new Exception(response.ErrorMessage);
                }
                lstMilesRewardDetails.ConvertFromDataset(response.dsResponse);
            }

            return milesRewardDetails;
        }

        #endregion

        #region IServiceAdapter Members


        public Recorder GetRecorder()
        {
            return this._recorder;
        }

        #endregion
    }
}