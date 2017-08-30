using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CCOHostIntegrationService.CouponEnquiryService;
using System.Data;

namespace Tesco.com.IntegrationServices
{
    // NOTE: If you change the class name "ClubcardCouponService" here, you must also update the reference to "ClubcardCouponService" in App.config.
    public class ClubcardCouponService : IClubcardCouponService
    {
        ClubcardCouponEnquiryServiceClient couponSVCClient;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="houseHoldID"></param>
        /// <param name="errorXml"></param>
        /// <param name="resultArray"></param>
        /// <returns></returns>
        public bool GetAvailableCoupons(long houseHoldID, out string errorXml, out List<CouponInformation> resultArray)
        {
            errorXml = string.Empty;
            resultArray = null;
            bool bResult = false;
            List<AvailableCoupons> availableCoupons = new List<AvailableCoupons>();
            List<CouponInformation> couponList = new List<CouponInformation>();

            try
            {
                couponSVCClient = new ClubcardCouponEnquiryServiceClient();

                //Call Clubcard coupon service to get the available coupons detail.
                availableCoupons = couponSVCClient.GetAvailableCoupons(new List<AvailableCouponRequest>() { new AvailableCouponRequest() { HouseHoldId = Convert.ToInt64(houseHoldID) } });

                if (availableCoupons.Count > 0 && availableCoupons[0].CouponList != null)
                {
                    resultArray = availableCoupons[0].CouponList;
                    bResult = true;
                }
            }
            catch (Exception exp)
            {

            }
            finally
            { }

            return bResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="houseHoldID"></param>
        /// <param name="errorXml"></param>
        /// <param name="resultArray"></param>
        /// <returns></returns>
        public bool GetRedeemedCoupons(long houseHoldID, out string errorXml, out List<CouponInformation> resultArray)
        {
            errorXml = string.Empty;
            resultArray = null;
            bool bResult = false;
            List<RedeemedCouponResponse> redeemedCoupons = new List<RedeemedCouponResponse>();
            List<CouponInformation> couponList = new List<CouponInformation>();

            try
            {
                couponSVCClient = new ClubcardCouponEnquiryServiceClient();
                redeemedCoupons = couponSVCClient.GetRedeemedCoupons(new List<RedeemedCouponRequest>() { new RedeemedCouponRequest() { HouseHoldId = Convert.ToInt64(houseHoldID) } });

                if (redeemedCoupons[0].CouponList != null && redeemedCoupons[0].CouponList.Count > 0)
                {
                    resultArray = redeemedCoupons[0].CouponList;
                    bResult = true;
                }
            }
            catch (Exception exp)
            {

            }
            finally
            { }

            return bResult;
        }
    }
}
