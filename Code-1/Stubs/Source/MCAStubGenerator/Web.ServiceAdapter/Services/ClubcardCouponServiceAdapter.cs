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
using Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class ClubcardCouponServiceAdapter : IServiceAdapter
    {
        Recorder _recorder = null;
        IClubcardCouponService _clubcardCouponServiceClient = new ClubcardCouponServiceClient();

        /// <summary>
        /// Constructor to create the instance of adaptor
        /// </summary>
        /// <param name="clubcardCouponService"></param>
        /// <param name="customerService"></param>
        /// <param name="logger"></param>
        public ClubcardCouponServiceAdapter(Recorder recorder)
        {
            this._recorder = recorder;
        }

        #region IServiceAdapter Members

        public MCAResponse Get(MCARequest request)
        {
            MCAResponse res = new MCAResponse();

            var operation = request.Parameters.Keys.Contains(ParameterNames.OPERATION_NAME) ? request.Parameters[ParameterNames.OPERATION_NAME].ToString() : string.Empty;
            switch (operation)
            {
                case OperationNames.GET_AVAILABLE_COUPONS:
                    int totalCoupons = 0;
                    res.Data = this.GetAvailableCoupons(out totalCoupons, request.Parameters[ParameterNames.HOUSEHOLD_ID].TryParse<Int64>());
                    res.OutParameters.Add(ParameterNames.TOTAL_COUPONS_VALUE, totalCoupons);

                    break;
                case OperationNames.GET_REDEEMED_COUPONS:
                    res.Data = this.GetRedeemedCoupons(request.Parameters[ParameterNames.HOUSEHOLD_ID].TryParse<Int64>(), request.Parameters[ParameterNames.CULTURE].TryParse<string>());
                    break;
            }
            res.Status = true;

            return res;
        }

        public Common.ResponseRecorder.Recorder GetRecorder()
        {
            return this._recorder;
        }

        #endregion

        #region Private Functions

        List<CouponDetails> GetAvailableCoupons(out int totalCoupons, long houseHoldId)
        {
            CouponDetailsList couponsList = new CouponDetailsList();
            List<CouponDetails> couponDetailList = new List<CouponDetails>();
            ClubcardCouponServices.CouponInformation[] availableCoupons = null;
            string errorXml = string.Empty;
            totalCoupons = 0;
            try
            {
                _clubcardCouponServiceClient.GetAvailableCoupons(out errorXml, out availableCoupons, out totalCoupons, houseHoldId);

                this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = availableCoupons.ToXmlString(), TotalCoupons = totalCoupons },
                      Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardCouponService.ToString(),
                      "GetAvailableCoupons", ResponseType.Xml);

                if (availableCoupons != null)
                {
                    string xml = availableCoupons.ToXmlString();
                    couponsList.ConvertFromXml(xml, typeof(ClubcardCouponServices.CouponInformation).Name);
                    couponDetailList = couponsList.List;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return couponDetailList;
        }

        public List<CouponDetails> GetRedeemedCoupons(long houseHoldId, string culture)
        {
            CouponDetailsList lstCouponDetails = new CouponDetailsList();
            List<CouponDetails> couponDetailList = new List<CouponDetails>();
            string errorXml = string.Empty;
            string resultXml = string.Empty;
            try
            {
                _clubcardCouponServiceClient.GetRedeemedCoupons(out errorXml, out resultXml, houseHoldId, culture);
                
                this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = resultXml.ToXmlString() },
                    Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardCouponService.ToString(),
                    "GetRedeemedCoupons", ResponseType.Xml);

                if (!string.IsNullOrEmpty(resultXml) && resultXml != "<NewDataSet />")
                {
                    lstCouponDetails.ConvertFromXml(resultXml);
                    couponDetailList = lstCouponDetails.List;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return couponDetailList;
        }

        #endregion

        #region IServiceAdapter Members

        

        #endregion
    }
}
