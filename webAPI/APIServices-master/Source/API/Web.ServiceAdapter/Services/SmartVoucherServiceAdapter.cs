using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.ServiceModel;
using System.Collections.Generic;
using System.Collections;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.SmartVoucherServices;
using Tesco.ClubcardProducts.MCA.API.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.ClubcardService;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Boost;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Points;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.API.Contracts;
using System.ComponentModel;

namespace Tesco.ClubcardProducts.MCA.API.ServiceAdapter.Services
{
    public class SmartVoucherServiceAdapter : BaseNGCAdapter, IServiceAdapter
    {
        ISmartVoucherServices _smartVoucherServiceClient = null;
        DateTime _dtStart = DateTime.UtcNow;

        public SmartVoucherServiceAdapter()
        {
        }

        public SmartVoucherServiceAdapter(string dotcomid, string uuid, string culture)
            : base(dotcomid, uuid, culture)
        {
            this._smartVoucherServiceClient = new SmartVoucherServicesClient();
        }

        #region IServiceAdapter Members

        public Dictionary<string, object> GetSupportedOperations()
        {
            return new Dictionary<string, object>() 
            { 
                {
                    "GetVoucherRewardDetails", new List<VoucherRewardDetails>(){
                        { 
                            new VoucherRewardDetails(){
                            } 
                        }
                    }
                },
                {
                    "GetUnusedVoucherDetails", new List<VoucherDetails>(){
                        { 
                            new VoucherDetails(){                                
                            } 
                        }
                    }
                },
                { 
                    "GetCustomerVoucherValCPS", new List<RewardAndPoints>(){
                        {
                            new RewardAndPoints(){
                            } 
                        }
                    }
                },
                { 
                    "GetUsedVoucherDetails", new List<VoucherUsageSummary>(){
                        {
                            new VoucherUsageSummary(){
                            } 
                        }
                    }
                },
                { 
                    "GetRewardDetailsMiles", new List<MilesRewardDetails>(){
                        {
                            new MilesRewardDetails(){
                            } 
                        }
                    }
                }
            };
        }

        public string GetName()
        {
            return "smartvoucherservice";
        }

        public APIResponse Execute(APIRequest request)
        {
            APIResponse response = new APIResponse();
            try
            {
                switch (request.operation.ToLower())
                {
                    case "getvoucherrewarddetails":
                        response.data = this.GetVoucherRewardDetails();
                        break;

                    case "getunusedvoucherdetails":
                        response.data = this.GetUnusedVoucherDetails();
                        break;

                    case "getcustomervouchervalcps":
                        response.data = this.GetCustomerVoucherValCPS(
                                                request.GetParameter<string>("stdate"),
                                                request.GetParameter<string>("endate"));
                        break;
                    case "getusedvoucherdetails":
                        response.data = this.GetUsedVoucherDetails();
                        break;
                    case "getrewarddetailsmiles":
                        response.data = this.GetRewardDetailsMiles(request.GetParameter<string>("reasoncode"));
                        break;
                }
            }
            catch (Exception ex)
            {
                response.errors.Add(new KeyValuePair<string, string>("ERR-SMARTVOUCHER-SERVICE", ex.ToString()));
            }
            finally
            {
                response.servicestats = this._internalStats.ToString();
            }
            return response;
        }

        #endregion IServiceAdapter Members

        #region Private Members

        private List<VoucherRewardDetails> GetVoucherRewardDetails()
        {
            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                string card = this.GetClubcardsByType(CardType.MyAccount).FirstOrDefault().ClubCardID;
                if (String.IsNullOrWhiteSpace(card))
                {
                    throw new Exception("Card details not available.");
                }

                VoucherRewardDetailsList voucherDetails = new VoucherRewardDetailsList();

                GetRewardDtlsRsp response = null;
                this._dtStart = DateTime.UtcNow;

                try
                {
                    response = _smartVoucherServiceClient.GetRewardDtls(card);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                if (response != null && response.dsResponse != null)
                {
                    voucherDetails.ConvertFromDataset(response.dsResponse);
                }
                return voucherDetails.VoucherRewardDetailList;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting status for GetRewardDetails call ", ex,
                    null);
            }
        }

        /// <summary>
        /// Method to get the Unused voucher details for the customer
        /// </summary>
        /// <param name="customerID">long customerID</param>
        /// <param name="cardNumber">long cardNumber</param>
        /// <param name="culture">string culture</param>
        /// <returns></returns>
        private List<VoucherDetails> GetUnusedVoucherDetails()
        {
            List<VoucherDetails> voucherDetailsList = new List<VoucherDetails>();
            
            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                string card = this.GetClubcardsByType(CardType.MyAccount).FirstOrDefault().ClubCardID;
                if (String.IsNullOrWhiteSpace(card))
                {
                    throw new Exception("Card details not available.");
                }

                GetUnusedVoucherDtlsRsp response = null;
                this._dtStart = DateTime.UtcNow;
                try
                {
                    response = _smartVoucherServiceClient.GetUnusedVoucherDtls(card);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }
                
                if (response != null && response.dsResponse != null)
                {
                    VoucherDetailsList lstvoucherDetailsList = new VoucherDetailsList();
                    lstvoucherDetailsList.ConvertFromDataset(response.dsResponse);
                    voucherDetailsList = lstvoucherDetailsList.VoucherDetailsListInstance;
                }
                return voucherDetailsList;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting status for GetUnusedVoucherDtls call ", ex,
                    null);
            }
        }

        /// <summary>
        /// Method to get the Rewards and Points for the collection period
        /// </summary>
        /// <param name="cardNumber">string cardNumber</param>
        /// <param name="stDate">string Collection Period Start Date</param>
        /// <param name="enDate">string Collection Period End Date</param>
        /// <returns></returns>
        private List<RewardAndPoints> GetCustomerVoucherValCPS(string stDate, string enDate)
        {
            List<RewardAndPoints> rewarAndPointsDetailList = new List<RewardAndPoints>();
            GetVoucherValAllCPSRsp response = new GetVoucherValAllCPSRsp();
            
            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                string card = this.GetClubcardsByType(CardType.MyAccount).FirstOrDefault().ClubCardID;
                if (String.IsNullOrWhiteSpace(card))
                {
                    throw new Exception("Card details not available.");
                }

                //Get the Clubcard voucher value from the webservice.          
                this._dtStart = DateTime.UtcNow;
                try
                {
                    response = _smartVoucherServiceClient.GetVoucherValCPS(card, stDate, enDate);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

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
                throw GeneralUtility.GetCustomException("Failed while getting status for GetVoucherValCPS call ", ex,
                    null);
            }
        }

        /// <summary>
        /// Method to get the used vouchers details for the customer
        /// </summary>
        /// <param name="clubcardNumber">string clubcardNumber</param>
        /// <returns></returns>
        private List<VoucherUsageSummary> GetUsedVoucherDetails()
        {
            List<VoucherUsageSummary> voucherRewardDetailsList = new List<VoucherUsageSummary>();
            VoucherUsageSummaryLst lstvoucherRewardDetailsList = new VoucherUsageSummaryLst();
            
            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                string card = this.GetClubcardsByType(CardType.MyAccount).FirstOrDefault().ClubCardID;
                if (String.IsNullOrWhiteSpace(card))
                {
                    throw new Exception("Card details not available.");
                }

                this._dtStart = DateTime.UtcNow;
                GetUsedVoucherDtlsRsp response = null;
                try
                {
                    response = _smartVoucherServiceClient.GetUsedVoucherDtls(card);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                if (response != null && response.dsResponse != null)
                {
                    lstvoucherRewardDetailsList.ConvertFromDataset(response.dsResponse);
                    voucherRewardDetailsList = lstvoucherRewardDetailsList.VoucherUsageSummaryLstInstance;
                }
               
                return voucherRewardDetailsList;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting status for GetUsedVoucherDtls call ", ex,
                       null);
            }            
        }

        private List<MilesRewardDetails> GetRewardDetailsMiles(string reasonCode)
        {
            List<MilesRewardDetails> milesRewardDetails = new List<MilesRewardDetails>();
            MilesRewardDetailsLst lstMilesRewardDetails = new MilesRewardDetailsLst();

            int iReasonCode = 0;
            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                string card = this.GetClubcardsByType(CardType.MyAccount).FirstOrDefault().ClubCardID;
                if (String.IsNullOrWhiteSpace(card))
                {
                    throw new Exception("Card details not available.");
                }

                Int32.TryParse(reasonCode, out iReasonCode);
                GetRewardDtlsMilesRsp response = null;
                this._dtStart = DateTime.UtcNow;
                try
                {
                    response = _smartVoucherServiceClient.GetRewardDtlsMiles(card, iReasonCode);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                if (response != null && response.dsResponse != null)
                {
                    lstMilesRewardDetails.ConvertFromDataset(response.dsResponse);
                }

                return lstMilesRewardDetails.MilesRewardDetailsLstInstance;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting status for GetRewardDtlsMiles call ", ex,
                    null);
            }
        }

        #endregion

    }
}