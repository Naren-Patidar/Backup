using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.ServiceModel;
using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.API.ServiceAdapter.ClubcardCouponServices;
using System.Collections;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerService;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.API.Common.Entities;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Ecoupon;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.API.Common;
using Tesco.ClubcardProducts.MCA.API.Contracts;

namespace Tesco.ClubcardProducts.MCA.API.ServiceAdapter.Services
{
    public class ClubcardCouponServiceAdapter : BaseNGCAdapter, IServiceAdapter
    {
        IClubcardCouponService _clubcardCouponServiceClient = null;
        double _internalStats = 0;
        DateTime _dtStart = DateTime.UtcNow;

        #region Constructors

        public ClubcardCouponServiceAdapter()
        {

        }

        public ClubcardCouponServiceAdapter(string dotcomid, string uuid, string culture)
            : base(dotcomid, uuid, culture)
        {
            _clubcardCouponServiceClient = new ClubcardCouponServiceClient();
        }

        /// <summary>
        /// Constructor to create the instance of adaptor
        /// </summary>
        /// <param name="clubcardCouponService"></param>
        /// <param name="customerService"></param>
        /// <param name="logger"></param>
        public ClubcardCouponServiceAdapter(IClubcardCouponService clubcardCouponService)
        {
            _clubcardCouponServiceClient = clubcardCouponService;
        }

        #endregion 

        #region IServiceAdapter Members

        public Dictionary<string, object> GetSupportedOperations()
        {
            return new Dictionary<string, object>() 
            { 
                {
                    "GetAvailableCoupons", new List<CouponDetails>{
                        { new CouponDetails() }
                    }
                },
                {
                    "GetRedeemedCoupons", new List<CouponDetails>{
                        { new CouponDetails() }
                    }
                }
            };
        }

        public string GetName()
        {
            return "couponservice";
        }

        public APIResponse Execute(APIRequest request)
        {
            APIResponse response = new APIResponse();
            try
            {
                switch (request.operation.ToLower())
                {
                    case "getavailablecoupons":
                        response.data = this.GetAvailableCoupons();
                        break;

                    case "getredeemedcoupons":
                        response.data = this.GetRedeemedCoupons();
                        break;

                }
            }
            catch (Exception ex)
            {
                response.errors.Add(new KeyValuePair<string, string>("ERR-COUPON-SERVICE", ex.ToString()));
            }
            finally
            {
                response.servicestats = this._internalStats.ToString();
            }
            return response;
        }

        #endregion

        #region Private Functions

        List<CouponDetails> GetAvailableCoupons()
        {
            CouponDetailsList couponsList = new CouponDetailsList();
            ClubcardCouponServices.CouponInformation[] availableCoupons = null;
            string errorXml = string.Empty;
            int totalCoupons = 0;

            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                long lHouseHoldId = this.GetHouseHoldID().TryParse<long>();
                if (lHouseHoldId == default(long))
                {
                    throw new Exception("Parameter houseHoldId is mandatory and must be passed for further processing.");
                }

                bool bService = false;
                this._dtStart = DateTime.UtcNow;

                try
                {
                    bService = this._clubcardCouponServiceClient.GetAvailableCoupons(out errorXml, out availableCoupons,
                                                out totalCoupons, lHouseHoldId);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);

                if (availableCoupons != null)
                {
                    string xml = availableCoupons.ToXmlString();
                    couponsList.ConvertFromXml(xml, typeof(ClubcardCouponServices.CouponInformation).Name);
                    return couponsList.List;
                }

                return new List<CouponDetails>();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in ClubcardCouponServiceAdapter while getting available coupons.", ex, null);
            }
        }

        public List<CouponDetails> GetRedeemedCoupons()
        {
            CouponDetailsList lstCouponDetails = new CouponDetailsList();
            List<CouponDetails> couponDetailList = new List<CouponDetails>();
            string errorXml = string.Empty;
            string resultXml = string.Empty;

            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                long lHouseHoldId = this.GetHouseHoldID().TryParse<long>();
                if (lHouseHoldId == default(long))
                {
                    throw new Exception("Parameter houseHoldId is mandatory and must be passed for further processing.");
                }

                bool bService = false;
                this._dtStart = DateTime.UtcNow;

                try
                {
                    bService = this._clubcardCouponServiceClient.GetRedeemedCoupons(out errorXml, out resultXml, lHouseHoldId, this.Culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                if (!string.IsNullOrEmpty(resultXml) && resultXml != "<NewDataSet />")
                {
                    lstCouponDetails.ConvertFromXml(resultXml);
                    couponDetailList = lstCouponDetails.List;
                    return couponDetailList;
                }

                return new List<CouponDetails>();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in ClubcardCouponServiceAdapter while getting redeemed coupons list.", ex, null);
            }
        }

        #endregion
    }
}
