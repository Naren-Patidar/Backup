using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.ServiceModel;
using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.ClubcardCouponServices;
using System.Collections;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerService;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class ClubcardCouponServiceAdapter : IServiceAdapter
    {

        IClubcardCouponService _clubcardCouponServiceClient = null;
        private readonly ILoggingService _logger = null;


        /// <summary>
        /// Constructor to create the instance of adaptor
        /// </summary>
        /// <param name="clubcardCouponService"></param>
        /// <param name="customerService"></param>
        /// <param name="logger"></param>
        public ClubcardCouponServiceAdapter(IClubcardCouponService clubcardCouponService, ILoggingService logger)
        {
            _clubcardCouponServiceClient = clubcardCouponService;
            _logger = logger;
        }



        #region IServiceAdapter Members

        public Common.Entities.Service.MCAResponse Get<T>(Common.Entities.Service.MCARequest request)
        {
            MCAResponse res = new MCAResponse();
            LogData _logData = new LogData();
            try
            {
                //_logger.Info("Get() Method of Class:{0} ", MethodBase.GetCurrentMethod().DeclaringType.FullName);
                var operation = request.Parameters.Keys.Contains(ParameterNames.OPERATION_NAME) ? request.Parameters[ParameterNames.OPERATION_NAME].ToString() : string.Empty;
                switch (operation)
                {
                    case OperationNames.GET_AVAILABLE_COUPONS:
                        _logData.RecordStep(string.Format("GET_AVAILABLE_COUPONS :{0}", OperationNames.GET_AVAILABLE_COUPONS));
                        int totalCoupons = 0;
                        res.Data = this.GetAvailableCoupons(out totalCoupons, request.Parameters[ParameterNames.HOUSEHOLD_ID].TryParse<Int64>());
                        res.OutParameters.Add(ParameterNames.TOTAL_COUPONS_VALUE, totalCoupons);

                        break;
                    case OperationNames.GET_REDEEMED_COUPONS:
                        _logData.RecordStep(string.Format("GET_REDEEMED_COUPONS :{0}", OperationNames.GET_REDEEMED_COUPONS));
                        res.Data = this.GetRedeemedCoupons(request.Parameters[ParameterNames.HOUSEHOLD_ID].TryParse<Int64>(), request.Parameters[ParameterNames.CULTURE].TryParse<string>());
                        break;
                }
                res.Status = true;

                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                
                res.Status = false;
                res.ErrorMessage = ex.Message;
                throw GeneralUtility.GetCustomException("Failed in ClubcardCouponServiceAdapter while calling private methods.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
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

        #region Private Functions

        List<CouponDetails> GetAvailableCoupons(out int totalCoupons, long houseHoldId)
        {
            LogData _logData = new LogData();
            CouponDetailsList couponsList = new CouponDetailsList();
            List<CouponDetails> couponDetailList = new List<CouponDetails>();
            ClubcardCouponServices.CouponInformation[] availableCoupons = null;
            string errorXml = string.Empty;
            totalCoupons = 0;
            try
            {
                _clubcardCouponServiceClient.GetAvailableCoupons(out errorXml, out availableCoupons, out totalCoupons, houseHoldId);
                if (availableCoupons != null)
                {
                    string xml = availableCoupons.ToXmlString();
                    couponsList.ConvertFromXml(xml, typeof(ClubcardCouponServices.CouponInformation).Name);
                    couponDetailList = couponsList.List;
                    _logData.RecordStep("couponDetailsList Count :" + couponDetailList.Count);
                }

                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in ClubcardCouponServiceAdapter while getting available coupons.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return couponDetailList;
        }

        public List<CouponDetails> GetRedeemedCoupons(long houseHoldId, string culture)
        {
            LogData _logData = new LogData();

            CouponDetailsList lstCouponDetails = new CouponDetailsList();
            List<CouponDetails> couponDetailList = new List<CouponDetails>();
            string errorXml = string.Empty;
            string resultXml = string.Empty;
            try
            {
                _clubcardCouponServiceClient.GetRedeemedCoupons(out errorXml, out resultXml, houseHoldId, culture);
                _logData.CaptureData("Redeemed coupons resultxml", resultXml);
                if (!string.IsNullOrEmpty(resultXml) && resultXml != "<NewDataSet />")
                {
                    lstCouponDetails.ConvertFromXml(resultXml);
                    couponDetailList = lstCouponDetails.List;
                    _logData.RecordStep("CouponDetails Count :" + couponDetailList.Count);
                    
                }
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in ClubcardCouponServiceAdapter while getting redeemed coupons list.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return couponDetailList;
        }

        #endregion
    }
}
